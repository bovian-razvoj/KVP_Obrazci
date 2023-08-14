function isNumberKey_int(s, e) {
    var charCode = e.htmlEvent.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        ASPxClientUtils.PreventEvent(e.htmlEvent);

    return true;
}

function isNumberKey_decimal(s, e) {
    var charCode = e.htmlEvent.keyCode;
    if (charCode != 44 && charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        ASPxClientUtils.PreventEvent(e.htmlEvent);

    return true;
}



ShowErrorPopUp = function (message, popup, title) {

    if (title === undefined || title == "") title = "Opozorilo";

    $('body').append('<div class="toggler popUpWrap"><div id="effect" class="ui-widget-content ui-corner-all popUpElement">' +
        '<h3 class="ui-widget-header ui-corner-all titleCenterAlign">' + title + '</h3>' +
    '<p>' +
      message +
    '</p>' +
  '<button type="button" id="btnClosePopUp">Zapri</button></div>' +
'</div>');

    if (popup === 1) {
        $("#effect").addClass("popUpElementWidth");
    }
    else {
        $("#effect").removeClass("popUpElementWidth");
    }

    //$("#effect").toggle('slow', 'swing');

    $("#btnClosePopUp").click(function () {
        //$("#effect").hide('blind', 'fast');
        //$(".toggler").hide('fade', 'slow');
        $(".toggler").remove();
    });
};

ShowSuccessPopUp = function (message, popup, title) {
    if (title == "") title = 'Bravo!';
    $('body').append('<div class="successToggler popUpWrap"><div id="effect" class="ui-widget-content ui-corner-all popUpElement">' +
        '<h3 class="ui-widget-header ui-corner-all titleCenterAlign">' + title + '</h3>' +
    '<p class="messagePopUp">' +
      message +
    '</p>' +
  '<button type="button" class="closeButton" id="btnCloseSuccessPopUp">Zapri</button></div>' +
'</div>');

    if (popup === 1) {
        $("#effect").addClass("popUpElementWidth");
    }
    else {
        $("#effect").removeClass("popUpElementWidth");
    }

    //$("#effect").toggle('slow', 'swing');

    $("#btnCloseSuccessPopUp").click(function () {
        //$("#effect").hide('blind', 'fast');
        //$(".toggler").hide('fade', 'slow');
        $(".successToggler").remove();
    });
};

function callback() {
    setTimeout(function () {
        $("#effect").removeAttr("style").hide().fadeIn();
    }, 1000);
};

HandleUserActionsOnTabs = function (gridView, btnAdd, btnEdit, btnDelete, objSender) {
    var elementName = objSender.name.substring(objSender.name.lastIndexOf('_') + 1, objSender.name.length);
    var parameter = "";

    switch (elementName) {
        case gridView.name.substring(gridView.name.lastIndexOf('_') + 1, gridView.name.length):
            parameter = "2";//row double click Edit
            break;
        case btnAdd.name.substring(btnAdd.name.lastIndexOf('_') + 1, btnAdd.name.length):
            parameter = "1";//Add
            break;
        case btnEdit.name.substring(btnEdit.name.lastIndexOf('_') + 1, btnEdit.name.length):
            parameter = "2";//Edit
            break;
        case btnDelete.name.substring(btnDelete.name.lastIndexOf('_') + 1, btnDelete.name.length):
            parameter = "3";//Delete
            break;
    }

    return parameter;

};


ShowTest = function () {
    alert("lalal");
};

CheckForSpecialCharacters = function (code) {
    for (var i = 0; i < code.length; i++) {
        if (code[i] >= 65 && code[i] <= 90) {
            continue;
        }
        else if (code.charCodeAt(i) == 352)//Š
            code = code.substr(0, i) + 'S' + code.substr(i + 1);
        else if (code.charCodeAt(i) == 268)//Č
            code = code.substr(0, i) + 'C' + code.substr(i + 1);
        else if (code.charCodeAt(i) == 381)//Ž
            code = code.substr(0, i) + 'Z' + code.substr(i + 1);
        else if (code.charCodeAt(i) == 262)//Ć
            code = code.substr(0, i) + 'C' + code.substr(i + 1);
    }
    return code;
};

/*Spinner Loader*/
ShowSpinnerLoader = function () {
    $('body').append('<div class="toggler popUpWrap"><button type="button" id="btnClosePopUp">Close</button></div><div id="effect" class="loader">' + '</div>');

    /*if (popup === 1) {
        $("#effect").addClass("popUpElementWidth");
    }
    else {
        $("#effect").removeClass("popUpElementWidth");
    }

    $("#effect").effect('bounce', 'slow');*/

    $("#btnClosePopUp").click(function () {
        $(".loader").hide('blind', 'fast');
        $(".toggler").hide('fade', 'slow');
    });
};

HideSpinnerLoader = function () {
    $(".loader").hide('blind', 'fast');
    $(".toggler").hide('fade', 'slow');
};

HamburgerMenu = function (show) {

    var effect = 'slide';
    var options = { direction: 'left' };
    var duration = 250;

    if (show) {
        $('.hamburger-wrap-close').removeClass('initial-hide-block');
        $('.hamburger-wrap').addClass('initial-hide-block');
        $('.main-menu-section').toggle(effect, options, duration);
    }
    else {
        $('.hamburger-wrap-close').addClass('initial-hide-block');
        $('.hamburger-wrap').removeClass('initial-hide-block');

        $('.main-menu-section').toggle(effect, options, duration, function () {
            $('.main-menu-section').css('display', '');
        });
    }
};

HamburgerMenuCloseOutsideClick = function (eventArg, container, hamburgerMenuBtn, closeMenuBtn) {

    if (hamburgerMenuBtn.is(':visible') || closeMenuBtn.is(':visible')) {
        // if the target of the click isn't the container nor a descendant of the container
        if ((!container.is(eventArg.target) && container.has(eventArg.target).length === 0) &&
            (!closeMenuBtn.is(eventArg.target) && closeMenuBtn.has(eventArg.target).length === 0)) {
            container.hide(400, function () {
                $('.main-menu-section').css('display', '');
            });
            $('.hamburger-wrap-close').addClass('initial-hide-block');
            $('.hamburger-wrap').removeClass('initial-hide-block');
        }
    }
};

ShowRemoveDropdownOutsideClick = function (eventArg, container, hamburgerMenuBtn, closeMenuBtn) {

    if (hamburgerMenuBtn.is(':visible')) {
        // if the target of the click isn't the container nor a descendant of the container
        if ((!container.is(eventArg.target) && container.has(eventArg.target).length === 0)) {
            $('#myDropDown').removeClass('show');
        }
    }
};

SetFocus = function (s, e) {
    try {
        var sender = s;
        sender.Focus();
    }
    catch (err)
    { }
};

InputFieldsValidation = function (gridLookupItems, inputFields, dateFields, memoFields, comboBoxItems, tokenBoxItems) {
    var procees = true;

    if (gridLookupItems != null) {
        for (var i = 0; i < gridLookupItems.length; i++) {
            var item = gridLookupItems[i];

            if (item.GetText() == null || item.GetText() == "Izberi... " || item.GetText() == "" || item.GetText() == "Izberi...  -  - ") {
                $(item.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    if (inputFields != null) {
        for (var i = 0; i < inputFields.length; i++) {

            var item = inputFields[i];
            if (item.GetText() == "") {
                $(item.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    if (dateFields != null) {
        for (var i = 0; i < dateFields.length; i++) {

            var item = dateFields[i];
            if (item.GetValue() == null) {
                $(item.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    if (memoFields != null) {
        for (var i = 0; i < memoFields.length; i++) {

            var item = memoFields[i];
            if (item.GetText() == "") {
                $(item.GetInputElement()).parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    if (comboBoxItems != null) {
        for (var i = 0; i < comboBoxItems.length; i++) {
            var item = comboBoxItems[i];
            if (item.GetSelectedItem() == null || item.GetSelectedItem().text == "Izberi... ") {
                $(item.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    if (tokenBoxItems != null) {
        for (var i = 0; i < tokenBoxItems.length; i++) {

            var item = tokenBoxItems[i];
            if (item.GetTokenCollection().length <= 0) {
                $(item.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                procees = false;
            }
        }
    }

    return procees;
};

getCookie = function (cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
};

OnClosePopupEventHandler_Vehicle = function (command) {
    switch (command) {
        case 'Potrdi':
            clientPopUpVehicle.Hide();
            clientVehicleCallback.PerformCallback("RefreshGrid");
            break;
        case 'Preklici':
            clientPopUpVehicle.Hide();
            break;
    }
}

OnFocusedRowGridLookupColumnValues = function (s, e, columns, OnGetRowValues) {
    var grid = s.GetGridView();
    grid.GetRowValues(grid.GetFocusedRowIndex(), columns, OnGetRowValues);
}

OnPopupCloseButtonClick = function (s) {
    s.PerformCallback('ClosePopupButtonClick');
}

ConfigureTabs = function (tabs, show, activeTab) {

    if (tabs != null) {
        var obj = JSON.parse(tabs);
        for (var i = 0; i < obj.length; i++) {
            var item = obj[i];
           
            if (show === 'True') {
                $('#' + item).show();
            }
            else {
                $('#' + item).hide();
            }
        }
    }
    
    if (activeTab != "") {
        $('#' + activeTab).addClass('active');
        $('#myKVPs').removeClass('active');
    }
};

GetUrlQueryStrings = function () {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
};

QueryStringsToObject = function () {
    var pairs = window.location.search.substring(1).split("&"),
      obj = {},
      pair,
      i;

    for (i in pairs) {
        if (pairs[i] === "") continue;

        pair = pairs[i].split("=");
        obj[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
    }

    return obj;
}

SerializeQueryStrings = function (obj) {
    var str = [];
    for (var p in obj)
        if (obj.hasOwnProperty(p)) {
            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
        }
    return str.join("&");
}

/*modalSaveKVP = function (saveOrSubmit) {
    $("#modal-btn-si").on("click", function () {
        callback(true);
        $("#saveKVPModal").modal('hide');
    });

    $("#modal-btn-no").on("click", function () {
        callback(false);
        $("#saveKVPModal").modal('hide');
    });
};*/