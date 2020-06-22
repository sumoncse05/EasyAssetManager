var combo = function () {
    var loadCombo = function (controlId, url, parameter, isDefaultRecordRequired) {
        $.ajax({
            url: url,
            type: 'get',
            async: false,
            data: parameter,
            success: function (res) {
                var data = res;
                $('#' + controlId).empty();
                $('#' + controlId).get(0).options.length = 0;
                if (isDefaultRecordRequired) {
                    $('#' + controlId).get(0).options[0] = new Option("------Select------", "");

                }
                if (data !== null && data.length > 0) {
                    $.each(data, function (index, item) {
                        $('#' + controlId).get(0).options[$('#' + controlId).get(0).options.length] = new Option(item.text, item.value);
                    });
                }
            },
            error: function () { }
        });
    };
    var loadComboStatic = function (controlId, data, isDefaultRecordRequired) {
        $('#' + controlId).empty();
        $('#' + controlId).get(0).options.length = 0;
        if (isDefaultRecordRequired) {
            $('#' + controlId).get(0).options[0] = new Option("------Select------", "");

        }
        if (data !== null && data.length > 0) {
            $.each(data, function (index, item) {
                $('#' + controlId).get(0).options[$('#' + controlId).get(0).options.length] = new Option(item.text, item.value);
            });
        }

    };
    var getGetCustomerComboWithSession = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    
    var getExcelFiletype = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "LOAN_PORTFOLIO",
            value: "1"
        };
        data.push(item);
        item = {
            text: "LOAN_TARGET",
            value: "2"
        };
        data.push(item);
        item = {
            text: "LOAN_CL",
            value: "3"
        };
        data.push(item);
        data.push(item);
        item = {
            text: "LOAN_WO",
            value: "4"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };



    return {
        getGetCustomerComboWithSession: getGetCustomerComboWithSession,
        getExcelFiletype: getExcelFiletype
    };
}();