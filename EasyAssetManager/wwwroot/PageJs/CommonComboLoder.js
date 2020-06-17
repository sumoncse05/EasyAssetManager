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
    var getGetWorkflowType = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getGetCustomerComboByAccountNo = function (controlId, url, accountNo, isDefaultRecordRequired) {
        var data = { accountNo: accountNo };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getRemittanceCompany = function (controlId, url, remitenceCompanyId, isDefaultRecordRequired) {
        var data = { remitenceCompanyId: remitenceCompanyId };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getBillers = function (controlId, url, pvc_billerid, isDefaultRecordRequired) {
        var data = { pvc_billerid: pvc_billerid };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getLimitUserTypeList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getLimitPackageList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getModuleList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getUserTypeList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getBarnchList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getUserBarnchList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getDepartmentList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getAgentList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getAgentOutletList = function (controlId, url, agent_id, isDefaultRecordRequired) {
        var data = { agent_id: agent_id };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getUserList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getAgentAccountList = function (controlId, url, agent_id, isDefaultRecordRequired) {
        var data = { agent_id: agent_id };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getDivisionList = function (controlId, url, isDefaultRecordRequired) {
        var data = {};
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getDistrictList = function (controlId, url, div_code, isDefaultRecordRequired) {
        var data = { div_code: div_code };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getThanaList = function (controlId, url, div_code, dist_code, isDefaultRecordRequired) {
        var data = { div_code: div_code, dist_code: dist_code };
        loadCombo(controlId, url, data, isDefaultRecordRequired);
    };
    var getLocation = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Rural",
            value: "01"
        };
        data.push(item);
        item = {
            text: "Urban",
            value: "02"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getStatus = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Enable",
            value: "Y"
        };
        data.push(item);
        item = {
            text: "Disable",
            value: "N"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getSex = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Male",
            value: "M"
        };
        data.push(item);
        item = {
            text: "Female",
            value: "F"
        };
        data.push(item);
        item = {
            text: "Other",
            value: "O"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };

    var getCustomerType = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Customer",
            value: "Y"
        };
        data.push(item);
        item = {
            text: "Non-Customer",
            value: "N"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getBillerStatic = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Palli Biddyut",
            value: "BC0001"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getBearerType = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Self",
            value: "01"
        };
        data.push(item);
        item = {
            text: "Bearer",
            value: "02"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getAccountType = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Indivisual",
            value: "I"
        };
        data.push(item);
        item = {
            text: "Company",
            value: "C"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getOperatingMode = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Single",
            value: "S"
        };
        data.push(item);
        item = {
            text: "Joint",
            value: "J"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getLimitScope = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Global",
            value: "G"
        };
        data.push(item);
        item = {
            text: "Specific",
            value: "S"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getActype = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Current",
            value: "01"
        };
        data.push(item);
        item = {
            text: "Savings",
            value: "02"
        };
        data.push(item);
        item = {
            text: "SND",
            value: "03"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getUserActiveStatus = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "Active",
            value: "Y"
        };
        data.push(item);
        var item = {
            text: "Inactive",
            value: "N"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getCheckBookRequisitionType = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "1st Cheque Book",
            value: "1st Cheque Book"
        };
        data.push(item);
        var item = {
            text: "2nd Cheque Book",
            value: "2nd Cheque Book"
        };
        data.push(item);
        var item = {
            text: "Next Cheque Book",
            value: "Next Cheque Book"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    var getCheckBookLeaves = function (controlId, isDefaultRecordRequired) {
        var data = [];
        var item = {
            text: "10",
            value: "10"
        };
        data.push(item);
        var item = {
            text: "25",
            value: "25"
        };
        data.push(item);
        var item = {
            text: "50",
            value: "50"
        };
        data.push(item);
        var item = {
            text: "75",
            value: "75"
        };
        data.push(item);
        var item = {
            text: "100",
            value: "100"
        };
        data.push(item);
        loadComboStatic(controlId, data, isDefaultRecordRequired);
    };
    return {
        getGetCustomerComboWithSession: getGetCustomerComboWithSession,
        getGetCustomerComboByAccountNo: getGetCustomerComboByAccountNo,
        getRemittanceCompany: getRemittanceCompany,
        getBillers: getBillers,
        getUserList: getUserList,
        getSex: getSex,
        getCustomerType: getCustomerType,
        getBillerStatic: getBillerStatic,
        getBearerType: getBearerType,
        getGetWorkflowType: getGetWorkflowType,
        getAccountType: getAccountType,
        getOperatingMode: getOperatingMode,
        getModuleList: getModuleList,
        getUserTypeList: getUserTypeList,
        getBarnchList: getBarnchList,
        getDepartmentList: getDepartmentList,
        getAgentList: getAgentList,
        getAgentOutletList: getAgentOutletList,
        getActype: getActype,
        getLimitScope: getLimitScope,
        getLimitUserTypeList: getLimitUserTypeList,
        getLimitPackageList: getLimitPackageList,
        getUserActiveStatus: getUserActiveStatus,
        getStatus: getStatus,
        getLocation: getLocation,
        getAgentAccountList: getAgentAccountList,
        getDivisionList: getDivisionList,
        getDistrictList: getDistrictList,
        getThanaList: getThanaList,
        getUserBarnchList: getUserBarnchList,
        getCheckBookRequisitionType: getCheckBookRequisitionType,
        getCheckBookLeaves: getCheckBookLeaves
    };
}();