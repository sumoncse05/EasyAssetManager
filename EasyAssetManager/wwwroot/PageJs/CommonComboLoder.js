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

    var getRmList = function (controlId, url, isDefaultRecordRequired)
    {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getBranchList = function (controlId, url, isDefaultRecordRequired)
    {
        alert('hi');
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getAreaList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getLoanProductList = function (controlId, url, loanType, isDefaultRecordRequired) {
        var data = { loanType: loanType};
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
        item = {
            text: "LOAN_WO",
            value: "4"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getLoanType = function (controlId, isDefaultRecordRequired)
    {
        var data = [];
        var item = {
            text: "Retail",
            value: "01"
        };
        data.push(item);
        item = {
            text: "SME",
            value: "02"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getCustomerCatagory = function (controlId, isDefaultRecordRequired)
    {
        var data = [];
        var item = {
            text: "General",
            value: "01"
        };
        data.push(item);
        item = {
            text: "Priority",
            value: "02"
        };
        data.push(item);
        
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getCustomerStatus = function (controlId, isDefaultRecordRequired)
    {
        var data = [];
        var item = {
            text: "Portfolio Upgraded",
            value: "05"
        };
        data.push(item);
        
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };

    var getReportName = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Asset at a glance",
            value: "1"
        };
        data.push(item);
        item = {
            text: "Asset Area Wise Report",
            value: "2"
        };
        data.push(item);
        item = {
            text: "Asset Branch Wise Report",
            value: "3"
        };
        data.push(item);
        item = {
            text: "Asset RM Wise Report",
            value: "4"
        };
        data.push(item);

        item = {
            text: "Asset BST Wise Report",
            value: "5"
        };
        data.push(item);
        item = {
            text: "Product Wise Loan Report",
            value: "6"
        };
        data.push(item);
        item = {
            text: "Year Wise Loan Report",
            value: "7"
        };
        data.push(item);
        item = {
            text: "Client Wise Loan Report",
            value: "8"
        };
        data.push(item);

        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };


    return {
        getRmList: getRmList,
        getExcelFiletype: getExcelFiletype,
        getLoanType: getLoanType,
        getCustomerCatagory: getCustomerCatagory,
        getCustomerStatus: getCustomerStatus,
        getBranchList: getBranchList,
        getAreaList: getAreaList,
        getLoanProductList: getLoanProductList,
        getReportName: getReportName
    };
}();