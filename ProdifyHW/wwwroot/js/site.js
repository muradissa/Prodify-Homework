// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    // take and save the client Id
    var currentUrl = window.location.href;
    var segments = currentUrl.split('/');
    var clientId = segments[segments.length - 1];

    let leftItems = [];
    let rightItems = [];
    let selectedLeft = [];
    let selectedRight = [];
    // clientBooks keep the ids of all the books that a customer has taken over
    let clientBooks = [];

    // Get All Available Books
    $.ajax({ 
        url: '/Book/GetAllAvailableBook',
        method: 'GET',
        dataType: 'json',
        success: function (response) {

            leftItems = response.map(book => ({
                key: book.bookID,
                value: book.name
            }));
            renderList();
        },
        error: function (xhr, status, error) {
            console.error('Error:', status, error);
        }
    });

    // Get All taken Books by X client
    $.ajax({
        url: `/Book/GetTakenClientBooks/` + clientId,
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            rightItems = response.map(book => ({
                key: book.bookID,
                value: book.name
            }));

            clientBooks = response.map(book => (
                book.bookID
            ));

            renderList();
        },
        error: function (xhr, status, error) {
            console.error('Error:', status, error);
        }
    });

    // add and remove selected books from the - left side
    const handleLeftClick = (item) => {
        const newSelected = [...selectedLeft];
        const selectedIndex = selectedLeft.indexOf(item);
        if (selectedIndex === -1) {
            newSelected.push(item);
        } else {
            newSelected.splice(selectedIndex, 1);
        }
        selectedLeft = newSelected;
        renderList();
    };

    // add and remove selected books from the - right side
    const handleRightClick = (item) => {
        const newSelected = [...selectedRight];
        const selectedIndex = selectedRight.indexOf(item);
        if (selectedIndex === -1) {
            newSelected.push(item);
        } else {
            newSelected.splice(selectedIndex, 1);
        }
        selectedRight = newSelected;
        renderList();
    };

    // Transfer the selected items from left side to right side - Take
    const handleLeftToRightClick = () => {
        rightItems = [...rightItems, ...selectedLeft];
        leftItems = leftItems.filter(item => !selectedLeft.includes(item));

        selectedLeft = [];
        renderList();
    };

    // Transfer the selected items from right side to left side - Return
    const handleRightToLeftClick = () => {
        leftItems = [...leftItems, ...selectedRight];
        rightItems = rightItems.filter(item => !selectedRight.includes(item));
        selectedRight = [];
        renderList();
    };

    // save button 
    const handleSaveBtnClick = () => {
        takeBooksFromLibrary()
        returnBooksToLibrary()
        renderList()
        alert("Saved successfully");
    }

    function takeBooksFromLibrary() {
        // Prepare the data to be sent
        let listOfBooksId = [];
        rightItems.forEach(item => listOfBooksId.push(item.key));
        const data = {
            ClientId: parseInt(clientId),
            ListOfBooksId: listOfBooksId
        }; 
        // request to take books
        $.ajax({
            url: '/Book/TakeBooksFromLibrary/' + clientId,
            type: 'PUT',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                //alert("Books taken successfully")
                console.log('Books taken successfully');
            },
            error: function (xhr, status, error) {
                console.log('Error taking books:', error);
            }
        });
    }

    function returnBooksToLibrary() {
        // Prepare the data to be sent
        var rightSideBooksId = [];
        rightItems.forEach(item => rightSideBooksId.push(item.key))
        const filteredArray = clientBooks.filter((item) => !rightSideBooksId.includes(item));
        const data = {
            ClientId: parseInt(clientId),
            ListOfBooksId: filteredArray
        }; 
        // request to return books
        $.ajax({
            url: '/Book/ReturnBooksToLibrary/' + clientId,
            type: 'PUT',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                //alert("Books return successfully")
                console.log('Books return successfully');
            },
            error: function (xhr, status, error) {
                console.log('Error return books:', error);
            }
        });
    }

    const renderList = () => {
        const $listContainer = $('<div></div>');
        const $listBody = $('<div class="list-body"></div>');
        //   left side | Available books
        const $leftList = $('<div class="list"></div>');
        $leftList.append('<h3>Available books</h3>');
        const $leftUl = $('<ul></ul>');
        leftItems.forEach(item => {
            const $li = $('<li></li>')
                .attr('key', item.key)
                .addClass(selectedLeft.includes(item) ? 'selected' : '')
                .text(`${item.key} - ${item.value}`)
                .click(() => handleLeftClick(item));
            $leftUl.append($li);
        });
        $leftList.append($leftUl);
        // two button to transfer books between lists
        const $listActions = $('<div class="list-actions"></div>');
        const $btn1 = $('<button>Take &gt;</button>').click(handleLeftToRightClick);
        const $btn2 = $('<button>&lt; Return</button>').click(handleRightToLeftClick);
        $listActions.append($btn1);
        $listActions.append($btn2);
        //   right side | taken books
        const $rightList = $('<div class="list"></div>');
        $rightList.append('<h3>My Books</h3>');
        const $rightUl = $('<ul></ul>');
        rightItems.forEach(item => {
            const $li = $('<li></li>')
                .attr('key', item.key)
                .addClass(selectedRight.includes(item) ? 'selected' : '')
                .text(`${item.key} - ${item.value}`)
                .click(() => handleRightClick(item));
            $rightUl.append($li);
        });
        $rightList.append($rightUl);
        // Save button
        const $divSaveBuuton = $('<div class="div-save-btn"></div>');
        const $saveButton = $('<button class="save-btn">Save</button>').click(handleSaveBtnClick);
        $divSaveBuuton.append($saveButton)

        $listBody.append($leftList);
        $listBody.append($listActions);
        $listBody.append($rightList);
        $listContainer.append($listBody);
        $listContainer.append($divSaveBuuton);

        $('#transferBooks').html($listContainer);
    };

    renderList();
}); 