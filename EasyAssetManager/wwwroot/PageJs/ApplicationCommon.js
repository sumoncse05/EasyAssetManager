var ApplicationCommon = function ()
{
    $(document).ajaxStart(function ()
    {
        showLoder();
    })
        .ajaxStop(function ()
        {
            hideLoder();
        });
    var isValidForm = function (formId)
    {
        formId = '#' + formId;
        $(formId).validate();
        return $(formId).valid();
    };
    var post = function (url, data, callback)
    {
        $.ajax({
            url: url,
            method: 'POST',
            data: data,
            success: callback,
            error: callback
        });
    };
    //url:string   //type:string   //data:object  // stringfy:bool   //async:bool //cache:bool
    var request = function (url, type, data, stringfy, async, cache, successCallBack)
    {
        type === null ? 'GET' : 'POST';
        var processData = false;
        if (type === 'GET')
        {
            processData = true;
        }
        stringfy === null ? false : true;
        async === null ? true : false;
        cache === null ? false : true;
        $.ajax({
            url: url,
            type: type,
            data: data,
            dataType: 'json',
            async: async,
            cache: cache,
            contentType: false,
            processData: processData,
            success: successCallBack,
            error: successCallBack
        });
    };
    var showLoder = function ()
    {
        var width = window.outerWidth;
        var height = window.outerHeight;
        var w = (width / 2) - 750;
        var h = (height / 2) - 150;
        var div = '<div id="ICustomLoder" style="height:' + height + 'px; width:' + width + 'px; background-color:rgba(0,0,0,0.5);z-index:999999;top:0;bottom:0;right:0;left:0;position:fixed;">' +
            '<div style="margin-left:' + w + 'px;margin-top:' + h + 'px;"><p style="color:lightgray; font-size:20px; text-align:center; line-height:75px;"><img src="/AssetManager/ApplicationTheme/assets/img/loading.gif"/></p></div>';
        $('body').append(div);
    };
    var hideLoder = function ()
    {
        $('#ICustomLoder').remove();
    };

    var getDataObj = function (className)
    {
        var obj = new Object();
        $('.' + className).each(function ()
        {
            var element = $(this);
            var name = element.attr('name');
            var id = element.attr('id'); if (name !== 'undefined')
            {
                if (element.is('input[type="text"]') || element.is('input[type="hidden"]') || element.is('select') || element.is('input[type="password"]'))
                {
                    obj[name] = element.val();
                }
                if (element.is('textarea'))
                {
                    if (element.hasClass('ckeditor'))
                    {
                        obj[name] = tinyMCE.get(id).getContent();
                    }
                    //formData.append(name, CKEDITOR.instances[name].getData());                 
                    else
                    {
                        obj[name] = element.val();
                    }
                }
                if (element.is('input[type="checkbox"]'))
                {
                    obj[name] = document.getElementById(id).checked;
                }
            }
        });
        return obj;
    };

    var getData = function (className)
    {
        var formData = new FormData();
        $('.' + className).each(function ()
        {
            var element = $(this);
            var name = element.attr('name');
            var id = element.attr('id'); if (name !== 'undefined')
            {
                if (element.is('input[type="text"]') || element.is('input[type="hidden"]') || element.is('select') || element.is('input[type="password"]'))
                {
                    formData.append(name, element.val());
                }
                if (element.is('textarea'))
                {
                    if (element.hasClass('ckeditor'))
                    {
                        formData.append(name, tinyMCE.get(id).getContent());
                    }
                    //formData.append(name, CKEDITOR.instances[name].getData());                 
                    else
                    {
                        formData.append(name, element.val());
                    }
                }
                if (element.is('input[type="checkbox"]'))
                {
                    formData.append(name, document.getElementById(id).checked);
                }
                if (element.is('input[type="radio"]'))
                {
                    formData.append(name, document.querySelector('input[name=' + name + ']:checked').value);
                }
                if (element.is('input[type="file"]'))
                {
                    if (element.hasClass('cover'))
                    {
                        formData.append('coverImage', document.getElementById(id).files[0]);
                    }
                    else
                    {
                        formData.append('uploadedFile', document.getElementById(id).files[0]);
                    }
                }
            }
        });
        return formData;
    };
    var setData = function (data, excludes)
    {
        if (excludes === null)
        {
            excludes = [];
        }
        for (var key in data)
        {
            // const upper = key.charAt(0) + key.substring(1);
            const upper = key;
            if (excludes.indexOf(key) === -1)
            {
                if ($('#' + upper).is('input[type="text"]') || $('#' + upper).is('input[type="hidden"]')
                    || $('#' + upper).is('select') || $(this).is('input[type="password"]'))
                {
                    $('#' + upper).val(data[key]);
                    if ($('#' + upper).hasClass('chosen-select')) {
                        $('#' + upper).val(data[key]).trigger("chosen:updated");
                    }
                   
                }
                if ($('#' + upper).is('textarea'))
                {
                    if ($('#' + upper).hasClass('ckeditor'))
                    {
                        //CKEDITOR.instances[key].setData(data[key]);
                        if (data[key])
                        {
                            tinyMCE.get(upper).setContent(data[key]);
                        }
                    }
                    else
                    {
                        $('#' + upper).val(data[key]);
                    }
                }
                if ($('#' + upper).is('input[type="checkbox"]'))
                {
                    document.getElementById(upper).checked = data[key];
                }
            }
        }
    };
    var setVData = function (data, excludes)
    {
        if (excludes === null)
        {
            excludes = [];
        }
        for (var key in data)
        {
            // const upper = key.charAt(0).toUpperCase() + key.substring(1);
            const upper = key;
            if (excludes.indexOf(key) === -1)
            {
                if ($('#V' + upper).is('input[type="text"]') || $('#V' + upper).is('input[type="hidden"]')
                    || $('#V' + upper).is('select') || $(this).is('input[type="password"]'))
                {
                    $('#V' + upper).val(data[key]);
                }
                if ($('#V' + upper).is('textarea'))
                {
                    if ($('#V' + upper).hasClass('ckeditor'))
                    {
                        //CKEDITOR.instances[key].setData(data[key]);
                        if (data[key])
                        {
                            tinyMCE.get('V' + upper).setContent(data[key]);
                        }
                    }
                    else
                    {
                        $('#V' + upper).val(data[key]);
                    }
                }
                if ($('#V' + upper).is('input[type="checkbox"]'))
                {
                    document.getElementById('V' + upper).checked = data[key];
                }
            }
        }
    };
    var clearFields = function (excludes, formId)
    {
        if (excludes)
        {
            $('input[type="text"],input[type="hidden"],input[type="password"]').each(function ()
            {
                var id = $(this).attr('id');
                if (id && excludes.indexOf(id) < 0)
                {
                    if (!($(this).hasClass('non-cleared')))
                    {
                        $(this).val('');
                    }
                } else
                {
                    if (!($(this).hasClass('non-cleared')))
                    {
                        $(this).val('');
                    }
                }
            });
            $('textarea').each(function ()
            {
                if ($(this).hasClass('ckeditor'))
                {
                    var id = $(this).attr('id');
                    if (id)
                        tinyMCE.get(id).setContent('');
                    //CKEDITOR.instances[id].setData('');
                } else
                {
                    $(this).val('');
                }
            });
            $('select').each(function ()
            {

                $(this).val('');
            });
        } else
        {

            $('input[type="text"],input[type="hidden"],input[type="password"],input[type="file"]').each(function ()
            {
                if (!($(this).hasClass('non-cleared')))
                {
                    $(this).val('');
                }
               
            });
            $('select').each(function ()
            {
                $(this).val('');
                if ($(this).hasClass('chosen-select')) {
                    $(this).val('').trigger("chosen:updated");
                }
            });
            $('textarea').each(function ()
            {
                if ($(this).hasClass('ckeditor'))
                {
                    var id = $(this).attr('id');
                    if (id)
                        tinyMCE.get(id).setContent('');
                    //CKEDITOR.instances[id].setData('');
                } else
                {
                    $(this).val('');
                }
            });
        }

        $('input[type="checkbox"]').each(function ()
        {
            var id = $(this).attr('id');
            if (id)
                document.getElementById(id).checked = false;
        });
        if (formId)
        {
            var validator = $("#" + formId).validate();
            validator.resetForm();
        }

    };
    var makePagination = function (tableId, headerrow,rowArray, colArray, title)
    {
        $('#' + tableId).DataTable({
            pageLength: 10,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                {
                    extend: 'excel',
                    title: title,
                    customize: function (xlsx)
                    {
                        var sSh = xlsx.xl['styles.xml'];
                        var lastXfIndex = $('cellXfs xf', sSh).length - 1;
                        //var n1 = '<numFmt formatCode="##0.0000%" numFmtId="300"/>';
                        var s1 = '<xf numFmtId="300" fontId="0" fillId="0" borderId="0" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyNumberFormat="1"/>';
                        var s2 = '<xf numFmtId="0" fontId="2" fillId="2" borderId="1" applyFont="1" applyFill="1" applyBorder="1" xfId="0" applyAlignment="1">' +
                            '<alignment horizontal="center" wrapText="1" /></xf>';
                        // sSh.childNodes[0].childNodes[0].innerHTML += n1;
                        sSh.childNodes[0].childNodes[5].innerHTML += s1 + s2;

                        var fourDecPlaces = lastXfIndex + 1;
                        var greyBoldCentered = lastXfIndex + 2;

                        var sheet = xlsx.xl.worksheets['sheet1.xml'];
                        var mergeCells = $('mergeCells', sheet);
                        for (var i = 0; i < rowArray.length; i++)
                        {
                            rowmerge(rowArray[i][0], rowArray[i][1], rowArray[i][2]);
                        }
                        for (var j = 0; j < colArray.length; j++)
                        {
                            colmerge(colArray[j][0], colArray[j][1], colArray[j][2]);
                        }
                        function rowmerge(fromCol, toCol, row)
                        {
                            mergeCells[0].appendChild(_createNode(sheet, 'mergeCell', {
                                attr: {
                                    ref: toColumnName(fromCol) + row + ':' + toColumnName(toCol - 1) + row
                                }
                            }));
                        }

                        function colmerge(fromRow, toRow, col)
                        {
                            mergeCells[0].appendChild(_createNode(sheet, 'mergeCell', {
                                attr: {
                                    ref: toColumnName(col) + fromRow + ':' + toColumnName(col) + toRow
                                }
                            }));
                        }

                        function _createNode(doc, nodeName, opts)
                        {
                            var tempNode = doc.createElement(nodeName);
                            if (opts)
                            {
                                if (opts.attr)
                                {
                                    $(tempNode).attr(opts.attr);
                                }
                                if (opts.children)
                                {
                                    $.each(opts.children, function (key, value)
                                    {
                                        tempNode.appendChild(value);
                                    });
                                }
                                if (opts.text !== null && opts.text !== undefined)
                                {
                                    tempNode.appendChild(doc.createTextNode(opts.text));
                                }
                            }
                            return tempNode;
                        }

                        //Function to fetch the cell name
                        function toColumnName(num)
                        {
                            for (var ret = '', a = 1, b = 26; (num -= a) >= 0; a = b, b *= 26)
                            {
                                ret = String.fromCharCode(parseInt((num % b) / a) + 65) + ret;
                            }
                            return ret;
                        }

                        $('row c', sheet).attr('s', '25');
                        $('row:nth-child(1) c', sheet).attr('s', 7);
                        for (var i = 2; i <= headerrow + 1; i++)
                        {
                            $('row:nth-child(' + i + ') c', sheet).attr('s', greyBoldCentered);
                        }

                    }

                }
            ]

        });
    };
    var showNotification = function (messageType, message)
    {
        messageType = messageType.toString().toLowerCase();

        if (messageType === 1 || messageType === '1' || messageType === 'success')
        {
            toastr.success(message);
        }
        else if (messageType === 2 || messageType === '2' || messageType === 'error')
        {
            toastr.error(message);
        }
        else if (messageType === 3 || messageType === '3' || messageType === 'warning')
        {
            toastr.warning(message);
        }
        else if (messageType === 4 || messageType === '4' || messageType === 'info')
        {
            toastr.info(message);
        }
    };
    var formateDate = function (jsonDate)
    {
        var year = jsonDate.substring(0, 4);
        var month = jsonDate.substring(5, 7);
        var day = jsonDate.substring(8, 10);
        var date = day + '/' + month + '/' + year;
        return date;
    };
    var formateJsonDate = function (jsonDate)
    {
        var d = new Date(parseInt(jsonDate.slice(6, -2)));
        var date = d.getDate() + '-' + getMonthNameInShortForm(1 + d.getMonth()) + '-' + d.getFullYear();
        return date;
    };
    var formateJsonDateTime = function (jsonDate)
    {
        var d = new Date(parseInt(jsonDate.slice(6, -2)));
        var hour = d.getHours() === 0 ? 12 : (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
        var min = d.getMinutes() < 10 ? '0' + d.getMinutes() : d.getMinutes();
        var ampm = d.getHours() < 12 ? 'AM' : 'PM'; hour = hour < 10 ? '0' + hour : hour;
        var time = hour + ':' + min + ' ' + ampm;
        var date = d.getDate() + '-' + getMonthNameInShortForm(1 + d.getMonth()) + '-' + d.getFullYear() + " " + time;
        return date;
    };

    var formateJsonDateForCalender = function (jsonDate, addDays)
    {
        var d = new Date(parseInt(jsonDate.slice(6, -2)));
        if (!addDays)
            addDays = 0;
        var month = 1 + d.getMonth();
        if (month < 10)
            month = "0" + month;
        var day = d.getDate() + addDays;
        if (day < 10)
            day = "0" + day;
        var date = d.getFullYear() + '-' + month + '-' + day;
        return date;
    };
    var getMonthNameInShortForm = function (monthNumber)
    {
        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        return months[monthNumber - 1];
    };

    var validateIndividualFieldManually = function (frmName, controllName, status)
    {
        if (!status)
        {
            $('#' + frmName).data('bootstrapValidator').updateStatus(controllName, 'NOT-VALIDATE').validateField(controllName);

        }
        else
        {
            $('#' + frmName).data('bootstrapValidator').updateStatus(controllName, 'VALID').validateField(controllName);
        }
    };
    var isNullOrEmpty = function (str)
    {

        if (str === null)
            return false;

        if (typeof str === 'undefined')
            return false;

        var string = str.toString();
        if (!string)
            return true;
        return string === null || string.match(/^ *$/) !== null;
    };
    var bootboxYesNo = function (confrimMessage, callbackFunction, parameters)
    {
        bootbox.confirm({
            message: confrimMessage,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result)
            {
                if (result === true)
                {

                    if (typeof callbackFunction === 'string')
                    {
                        _callCallbackFuncation(callbackFunction, parameters);
                    }
                    else
                    {
                        if (callbackFunction)
                        {
                            callbackFunction();
                        }
                    }
                }
            }
        });

    };
    String.prototype.strReverse = function ()
    {
        var newstring = "";
        for (var s = 0; s < this.length; s++)
        {
            newstring = this.charAt(s) + newstring;
        }
        return newstring;
        //strOrig = ' texttotrim ';
        //strReversed = strOrig.revstring();
    };
    var initializeFormValidation = function ()
    {
        $.validate({
            modules: 'location, date, security, file',
        });
    };
  
    var forceModalClose = function ()
    {
        $('body').removeClass('modal-open');
        $(".modal-backdrop").remove();
        $('body').removeAttr('style');
    };
    var reportTitleAndHeaderCustomize = function () {
        var title=$("#reportName option:selected").text();
        if ($('#loantype').val() === "01") {
            $('#reportHeader').text("Retail " + title);
            title = "Retail " + title;
            $('#headerChange').text("Retail");
        } else if ($('#loantype').val() === "02") {
            $('#reportHeader').text("SME " + title);
            title = "SME " + title;
            $('#headerChange').text("Small");
        }
        var res = $('#toDate').val().split("/");
        var year = parseInt(res[2]);
        $('#headercurrentYear1').text(year);
        $('#headercurrentYear2').text(year);
        $('#headercurrentYear3').text(year);
        $('#headercurrentYear4').text(year);

        $('#headerpreviousYear1').text(year-1);
        $('#headerpreviousYear2').text(year-1);
        return title;
    };
    return {
        init: function ()
        {
        },
        isValidForm: isValidForm,
        request: request,
        getData: getData,
        getDataObj: getDataObj,
        setData: setData,
        clearFields: clearFields,
        makePagination: makePagination,
        showNotification: showNotification,
        formateJsonDate: formateJsonDate,
        formateJsonDateTime: formateJsonDateTime,
        formateJsonDateForCalender: formateJsonDateForCalender,
        validateIndividualFieldManually: validateIndividualFieldManually,
        post: post,
        isNullOrEmpty: isNullOrEmpty,
        bootboxYesNo: bootboxYesNo,
        formateDate: formateDate,
        getMonthNameInShortForm: getMonthNameInShortForm,
        initializeFormValidation: initializeFormValidation,
        showLoder: showLoder,
        forceModalClose: forceModalClose,
        reportTitleAndHeaderCustomize: reportTitleAndHeaderCustomize
    };
}();