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
            '<div style="margin-left:' + w + 'px;margin-top:' + h + 'px;"><p style="color:lightgray; font-size:20px; text-align:center; line-height:75px;"><img src="/ApplicationTheme/assets/img/loading.gif"/></p></div>';
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
    var makePagination = function (tableId)
    {
        $('#' + tableId).DataTable({
            pageLength: 10,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'copy' },
                { extend: 'csv' },
                { extend: 'excel', title: 'ExampleFile' },
                { extend: 'pdf', title: 'ExampleFile' },

                {
                    extend: 'print',
                    customize: function (win)
                    {
                        $(win.document.body).addClass('white-bg');
                        $(win.document.body).css('font-size', '10px');

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
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
        var date = day + '-' + ERPApplication.getMonthNameInShortForm(month) + '-' + year;
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

    var printMe = function (content)
    {
        var mywindow = window.open('', '');

        mywindow.document.write('<html><head><title></title>  <link rel="stylesheet" href="/assets/global/plugins/bootstrap/css/bootstrap.min.css" /> ');

        mywindow.document.write('</head><body >');
        if (content)
        {
            mywindow.document.write(content);
        }
        mywindow.document.write('</body></html>');
        setTimeout(function ()
        {
            mywindow.focus();
            mywindow.print();
            mywindow.close();
        }, 350);
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
    var showCustomerModal = function (controlId, nextCallMethod)
    {
        $('#modal_customer_Serch').modal('show');
        $('#txtControlId').val(controlId);
        $('#txtMethodName').val(nextCallMethod);
    };
    var loadCustomers = function (accountNo)
    {
        $('#customer_Serach').empty();
        var url = '/Common/GetCustomers';
        $('#customer_Serach').load(url + '?accountNo=' + accountNo);
    };

    var addRow2Table = function (table_id, jsonArr, par_aoColumnsDef, par_dropdowns)
    {
        var newObj = {};
        $.each(par_aoColumnsDef, function (index, value)
        {
            newObj[value.title] = '';
        });
        jsonArr.push(newObj);
        //loadTable(table_id, jsonArr, par_aoColumnsDef, par_dropdowns);
    };
    var loadTable = function (table_id, jsonArr, par_aoColumnsDef, par_dropdowns)
    {
        var dataset = [];

        $.each(jsonArr, function (index, value)
        {
            value.datatable_id = index;
            var newObj = {};
            $.each(value, function (key, val)
            {
                newObj[key] = val;
            });
            dataset.push(newObj);
        });

        var aoColumnsDef = [];
        $.each(par_aoColumnsDef, function (index, value)
        {
            var colObj;
            if (value.element)
                colObj = {
                    "mData": null,
                    "sTitle": value.title,
                    "bSortable": false,
                    "mRender": function (data, type, full)
                    {
                        return value.element.replace(/<index>/g, data.datatable_id);
                    }
                }
            else colObj = {
                "mData": index,
                "sTitle": value.title
            }
            aoColumnsDef.push(colObj);
        });
        $('#' + table_id).DataTable({
            data: dataset,
            "destroy": true,
            searching: false,
            paging: false,
            info: false,
            "aoColumns": aoColumnsDef,
        });
        $.each(par_dropdowns, function (id, dropdown)
        {
            $.each(jsonArr, function (index, val)
            {
                var mySelect = $('#' + dropdown.id + '' + val.datatable_id);
                $.each(dropdown.source, function (key, value)
                {
                    var $option = $("<option/>", {
                        value: key,
                        text: value
                    });
                    mySelect.append($option);
                });
                if (jsonArr[val.datatable_id][dropdown.id])
                    mySelect.val(jsonArr[val.datatable_id][dropdown.id]);
                else mySelect.val('0');
                $(mySelect).on('change', function ()
                {
                    jsonArr[val.datatable_id][dropdown.id] = this.value;
                });
            });
        });
    };
    var loadPieChart = function (id, data, title, subtitle)
    {
        require.config({
            paths: {
                echarts: '/ApplicationBaseTheme/assets/js/plugins/visualization/echarts'
            }
        });
        require(
            [
                'echarts',
                'echarts/theme/limitless',
                'echarts/chart/pie'
            ],
            function (ec, limitless)
            {
                var basic_pie = ec.init(document.getElementById(id), limitless);
                var column = [];
                $.each(data, function (index, value)
                {
                    column.push(data[index].name);
                });
                basic_pie_options = {

                    // Add title
                    title: {
                        text: title,
                        subtext: subtitle,
                        x: 'center'
                    },

                    // Add tooltip
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b}: {c} ({d}%)"
                    },

                    // Add legend
                    /*legend: {
                        orient: 'horizontal',
                        x: 'bottom',
                        data: column
                    },*/


                    // Enable drag recalculate
                    calculable: true,

                    // Add series
                    series: [{
                        name: title,
                        type: 'pie',
                        radius: '50%',
                        center: ['40%', '50%'],
                        data: data
                    }]
                };

                basic_pie.setOption(basic_pie_options);

            }
        );
    };
    var loadBarHierarchy = function (control_id, dataPath)
    {

        // Initialize chart
        stackedMultiples('#' + control_id, 200);

        // Chart setup
        function stackedMultiples(element, height)
        {


            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = { top: 25, right: 5, bottom: 5, left: 75 },
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5,
                barHeight = 30,
                duration = 750,
                delay = 25;



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.scale.linear()
                .range([0, width]);

            // Colors
            var color = d3.scale.ordinal()
                .range(["#26A69A", "#ccc"]);



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("top");



            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3Container.append("svg");

            // Add SVG group
            var svg = container
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


            // Construct chart layout
            // ------------------------------

            // Partition
            var partition = d3.layout.partition()
                .value(function (d) { return d.size; });



            // Load data
            // ------------------------------            
            d3.json(dataPath, function (error, root)
            {
                partition.nodes(root);
                x.domain([0, root.value]).nice();
                down(root, 0);
            });


            //
            // Append chart elements
            //

            // Add background bars
            svg.append("rect")
                .attr("class", "d3-bars-background")
                .attr("width", width)
                .attr("height", height)
                .style("fill", "#fff")
                .on("click", up);


            // Append axes
            // ------------------------------

            // Horizontal
            svg.append("g")
                .attr("class", "d3-axis d3-axis-horizontal d3-axis-strong");


            // Append bars
            // ------------------------------

            // Create hierarchical structure
            function down(d, i)
            {
                if (!d.children || this.__transition__) return;
                var end = duration + d.children.length * delay;

                // Mark any currently-displayed bars as exiting.
                var exit = svg.selectAll(".enter")
                    .attr("class", "exit");

                // Entering nodes immediately obscure the clicked-on bar, so hide it.
                exit.selectAll("rect").filter(function (p) { return p === d; })
                    .style("fill-opacity", 1e-6);

                // Enter the new bars for the clicked-on data.
                // Per above, entering bars are immediately visible.
                var enter = bar(d)
                    .attr("transform", stack(i))
                    .style("opacity", 1);

                // Have the text fade-in, even though the bars are visible.
                // Color the bars as parents; they will fade to children if appropriate.
                enter.select("text").style("fill-opacity", 1e-6);
                enter.select("rect").style("fill", color(true));

                // Update the x-scale domain.
                x.domain([0, d3.max(d.children, function (d) { return d.value; })]).nice();

                // Update the x-axis.
                svg.selectAll(".d3-axis-horizontal").transition()
                    .duration(duration)
                    .call(xAxis);

                // Transition entering bars to their new position.
                var enterTransition = enter.transition()
                    .duration(duration)
                    .delay(function (d, i) { return i * delay; })
                    .attr("transform", function (d, i) { return "translate(0," + barHeight * i * 1.2 + ")"; });

                // Transition entering text.
                enterTransition.select("text")
                    .style("fill-opacity", 1);

                // Transition entering rects to the new x-scale.
                enterTransition.select("rect")
                    .attr("width", function (d) { return x(d.value); })
                    .style("fill", function (d) { return color(!!d.children); });

                // Transition exiting bars to fade out.
                var exitTransition = exit.transition()
                    .duration(duration)
                    .style("opacity", 1e-6)
                    .remove();

                // Transition exiting bars to the new x-scale.
                exitTransition.selectAll("rect")
                    .attr("width", function (d) { return x(d.value); });

                // Rebind the current node to the background.
                svg.select(".d3-bars-background")
                    .datum(d)
                    .transition()
                    .duration(end);

                d.index = i;
            }

            // Return to parent level
            function up(d)
            {
                if (!d.parent || this.__transition__) return;
                var end = duration + d.children.length * delay;

                // Mark any currently-displayed bars as exiting.
                var exit = svg.selectAll(".enter")
                    .attr("class", "exit");

                // Enter the new bars for the clicked-on data's parent.
                var enter = bar(d.parent)
                    .attr("transform", function (d, i) { return "translate(0," + barHeight * i * 1.2 + ")"; })
                    .style("opacity", 1e-6);

                // Color the bars as appropriate.
                // Exiting nodes will obscure the parent bar, so hide it.
                enter.select("rect")
                    .style("fill", function (d) { return color(!!d.children); })
                    .filter(function (p) { return p === d; })
                    .style("fill-opacity", 1e-6);

                // Update the x-scale domain.
                x.domain([0, d3.max(d.parent.children, function (d) { return d.value; })]).nice();

                // Update the x-axis.
                svg.selectAll(".d3-axis-horizontal").transition()
                    .duration(duration)
                    .call(xAxis);

                // Transition entering bars to fade in over the full duration.
                var enterTransition = enter.transition()
                    .duration(end)
                    .style("opacity", 1);

                // Transition entering rects to the new x-scale.
                // When the entering parent rect is done, make it visible!
                enterTransition.select("rect")
                    .attr("width", function (d) { return x(d.value); })
                    .each("end", function (p) { if (p === d) d3.select(this).style("fill-opacity", null); });

                // Transition exiting bars to the parent's position.
                var exitTransition = exit.selectAll("g").transition()
                    .duration(duration)
                    .delay(function (d, i) { return i * delay; })
                    .attr("transform", stack(d.index));

                // Transition exiting text to fade out.
                exitTransition.select("text")
                    .style("fill-opacity", 1e-6);

                // Transition exiting rects to the new scale and fade to parent color.
                exitTransition.select("rect")
                    .attr("width", function (d) { return x(d.value); })
                    .style("fill", color(true));

                // Remove exiting nodes when the last child has finished transitioning.
                exit.transition()
                    .duration(end)
                    .remove();

                // Rebind the current parent to the background.
                svg.select(".d3-bars-background")
                    .datum(d.parent)
                    .transition()
                    .duration(end);
            }

            // Creates a set of bars for the given data node, at the specified index.
            function bar(d)
            {
                var bar = svg.insert("g", ".d3-axis-vertical")
                    .attr("class", "enter")
                    .attr("transform", "translate(0,5)")
                    .selectAll("g")
                    .data(d.children)
                    .enter()
                    .append("g")
                    .style("cursor", function (d) { return !d.children ? null : "pointer"; })
                    .on("click", down);

                bar.append("text")
                    .attr("x", -6)
                    .attr("y", barHeight / 2)
                    .attr("dy", ".35em")
                    .style("text-anchor", "end")
                    .text(function (d) { return d.name; });

                bar.append("rect")
                    .attr("width", function (d) { return x(d.value); })
                    .attr("height", barHeight);

                return bar;
            }

            // A stateful closure for stacking bars horizontally.
            function stack(i)
            {
                var x0 = 0;
                return function (d)
                {
                    var tx = "translate(" + x0 + "," + barHeight * i * 1.2 + ")";
                    x0 += x(d.value);
                    return tx;
                };
            }



            // Resize chart
            // ------------------------------

            // Call function on window resize
            $(window).on('resize', resize);

            // Call function on sidebar width change
            $('.sidebar-control').on('click', resize);

            // Resize function
            // 
            // Since D3 doesn't support SVG resize by default,
            // we need to manually specify parts of the graph that need to 
            // be updated on window resize
            function resize()
            {

                // Layout variables
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right;


                // Layout
                // -------------------------

                // Main svg width
                container.attr("width", width + margin.left + margin.right);

                // Width of appended group
                svg.attr("width", width + margin.left + margin.right);


                // Axes
                // -------------------------

                // Horizontal range
                x.range([0, width]);

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);


                // Chart elements
                // -------------------------

                // Bars
                svg.selectAll('.enter rect').attr("width", function (d) { return x(d.value); });
            }
        }
    };
    var chkPass = function (pwd)
    {
        var oScorebar = $("scorebar");
        var oScore = $("score");
        var oComplexity = $("complexity");
        // Simultaneous variable declaration and value assignment aren't supported in IE apparently
        // so I'm forced to assign the same value individually per var to support a crappy browser *sigh*
        var nScore = 0, nLength = 0, nAlphaUC = 0, nAlphaLC = 0, nNumber = 0, nSymbol = 0, nMidChar = 0, nRequirements = 0, nAlphasOnly = 0, nNumbersOnly = 0, nUnqChar = 0, nRepChar = 0, nRepInc = 0, nConsecAlphaUC = 0, nConsecAlphaLC = 0, nConsecNumber = 0, nConsecSymbol = 0, nConsecCharType = 0, nSeqAlpha = 0, nSeqNumber = 0, nSeqSymbol = 0, nSeqChar = 0, nReqChar = 0, nMultConsecCharType = 0;
        var nMultRepChar = 1, nMultConsecSymbol = 1;
        var nMultMidChar = 2, nMultRequirements = 2, nMultConsecAlphaUC = 2, nMultConsecAlphaLC = 2, nMultConsecNumber = 2;
        var nReqCharType = 3, nMultAlphaUC = 3, nMultAlphaLC = 3, nMultSeqAlpha = 3, nMultSeqNumber = 3, nMultSeqSymbol = 3;
        var nMultLength = 4, nMultNumber = 4;
        var nMultSymbol = 6;
        var nTmpAlphaUC = "", nTmpAlphaLC = "", nTmpNumber = "", nTmpSymbol = "";
        var sAlphaUC = "0", sAlphaLC = "0", sNumber = "0", sSymbol = "0", sMidChar = "0", sRequirements = "0", sAlphasOnly = "0", sNumbersOnly = "0", sRepChar = "0", sConsecAlphaUC = "0", sConsecAlphaLC = "0", sConsecNumber = "0", sSeqAlpha = "0", sSeqNumber = "0", sSeqSymbol = "0";
        var sAlphas = "abcdefghijklmnopqrstuvwxyz";
        var sNumerics = "01234567890";
        var sSymbols = ")!@#$%^&*()";
        var sComplexity = "Too Short";
        var sStandards = "Below";
        var nMinPwdLen = 8;
        if (document.all) { var nd = 0; } else { var nd = 1; }
        if (pwd)
        {
            nScore = parseInt(pwd.length * nMultLength);
            nLength = pwd.length;
            var arrPwd = pwd.replace(/\s+/g, "").split(/\s*/);
            var arrPwdLen = arrPwd.length;

            /* Loop through password to check for Symbol, Numeric, Lowercase and Uppercase pattern matches */
            for (var a = 0; a < arrPwdLen; a++)
            {
                if (arrPwd[a].match(/[A-Z]/g))
                {
                    if (nTmpAlphaUC !== "") { if ((nTmpAlphaUC + 1) == a) { nConsecAlphaUC++; nConsecCharType++; } }
                    nTmpAlphaUC = a;
                    nAlphaUC++;
                }
                else if (arrPwd[a].match(/[a-z]/g))
                {
                    if (nTmpAlphaLC !== "") { if ((nTmpAlphaLC + 1) == a) { nConsecAlphaLC++; nConsecCharType++; } }
                    nTmpAlphaLC = a;
                    nAlphaLC++;
                }
                else if (arrPwd[a].match(/[0-9]/g))
                {
                    if (a > 0 && a < (arrPwdLen - 1)) { nMidChar++; }
                    if (nTmpNumber !== "") { if ((nTmpNumber + 1) == a) { nConsecNumber++; nConsecCharType++; } }
                    nTmpNumber = a;
                    nNumber++;
                }
                else if (arrPwd[a].match(/[^a-zA-Z0-9_]/g))
                {
                    if (a > 0 && a < (arrPwdLen - 1)) { nMidChar++; }
                    if (nTmpSymbol !== "") { if ((nTmpSymbol + 1) == a) { nConsecSymbol++; nConsecCharType++; } }
                    nTmpSymbol = a;
                    nSymbol++;
                }
                /* Internal loop through password to check for repeat characters */
                var bCharExists = false;
                for (var b = 0; b < arrPwdLen; b++)
                {
                    if (arrPwd[a] == arrPwd[b] && a != b)
                    { /* repeat character exists */
                        bCharExists = true;
                        /*
                        Calculate icrement deduction based on proximity to identical characters
                        Deduction is incremented each time a new match is discovered
                        Deduction amount is based on total password length divided by the
                        difference of distance between currently selected match
                        */
                        nRepInc += Math.abs(arrPwdLen / (b - a));
                    }
                }
                if (bCharExists)
                {
                    nRepChar++;
                    nUnqChar = arrPwdLen - nRepChar;
                    nRepInc = (nUnqChar) ? Math.ceil(nRepInc / nUnqChar) : Math.ceil(nRepInc);
                }
            }

            /* Check for sequential alpha string patterns (forward and reverse) */
            for (var s = 0; s < 23; s++)
            {
                var sFwd = sAlphas.substring(s, parseInt(s + 3));
                var sRev = sFwd.strReverse();
                if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqAlpha++; nSeqChar++; }
            }

            /* Check for sequential numeric string patterns (forward and reverse) */
            for (var s = 0; s < 8; s++)
            {
                var sFwd = sNumerics.substring(s, parseInt(s + 3));
                var sRev = sFwd.strReverse();
                if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqNumber++; nSeqChar++; }
            }

            /* Check for sequential symbol string patterns (forward and reverse) */
            for (var s = 0; s < 8; s++)
            {
                var sFwd = sSymbols.substring(s, parseInt(s + 3));
                var sRev = sFwd.strReverse();
                if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqSymbol++; nSeqChar++; }
            }

            /* Modify overall score value based on usage vs requirements */

            /* General point assignment */
            if (nAlphaUC > 0 && nAlphaUC < nLength)
            {
                nScore = parseInt(nScore + ((nLength - nAlphaUC) * 2));
                sAlphaUC = "+ " + parseInt((nLength - nAlphaUC) * 2);
            }
            if (nAlphaLC > 0 && nAlphaLC < nLength)
            {
                nScore = parseInt(nScore + ((nLength - nAlphaLC) * 2));
                sAlphaLC = "+ " + parseInt((nLength - nAlphaLC) * 2);
            }
            if (nNumber > 0 && nNumber < nLength)
            {
                nScore = parseInt(nScore + (nNumber * nMultNumber));
                sNumber = "+ " + parseInt(nNumber * nMultNumber);
            }
            if (nSymbol > 0)
            {
                nScore = parseInt(nScore + (nSymbol * nMultSymbol));
                sSymbol = "+ " + parseInt(nSymbol * nMultSymbol);
            }
            if (nMidChar > 0)
            {
                nScore = parseInt(nScore + (nMidChar * nMultMidChar));
                sMidChar = "+ " + parseInt(nMidChar * nMultMidChar);
            }

            /* Point deductions for poor practices */
            if ((nAlphaLC > 0 || nAlphaUC > 0) && nSymbol === 0 && nNumber === 0)
            {  // Only Letters
                nScore = parseInt(nScore - nLength);
                nAlphasOnly = nLength;
                sAlphasOnly = "- " + nLength;
            }
            if (nAlphaLC === 0 && nAlphaUC === 0 && nSymbol === 0 && nNumber > 0)
            {  // Only Numbers
                nScore = parseInt(nScore - nLength);
                nNumbersOnly = nLength;
                sNumbersOnly = "- " + nLength;
            }
            if (nRepChar > 0)
            {  // Same character exists more than once
                nScore = parseInt(nScore - nRepInc);
                sRepChar = "- " + nRepInc;
            }
            if (nConsecAlphaUC > 0)
            {  // Consecutive Uppercase Letters exist
                nScore = parseInt(nScore - (nConsecAlphaUC * nMultConsecAlphaUC));
                sConsecAlphaUC = "- " + parseInt(nConsecAlphaUC * nMultConsecAlphaUC);
            }
            if (nConsecAlphaLC > 0)
            {  // Consecutive Lowercase Letters exist
                nScore = parseInt(nScore - (nConsecAlphaLC * nMultConsecAlphaLC));
                sConsecAlphaLC = "- " + parseInt(nConsecAlphaLC * nMultConsecAlphaLC);
            }
            if (nConsecNumber > 0)
            {  // Consecutive Numbers exist
                nScore = parseInt(nScore - (nConsecNumber * nMultConsecNumber));
                sConsecNumber = "- " + parseInt(nConsecNumber * nMultConsecNumber);
            }
            if (nSeqAlpha > 0)
            {  // Sequential alpha strings exist (3 characters or more)
                nScore = parseInt(nScore - (nSeqAlpha * nMultSeqAlpha));
                sSeqAlpha = "- " + parseInt(nSeqAlpha * nMultSeqAlpha);
            }
            if (nSeqNumber > 0)
            {  // Sequential numeric strings exist (3 characters or more)
                nScore = parseInt(nScore - (nSeqNumber * nMultSeqNumber));
                sSeqNumber = "- " + parseInt(nSeqNumber * nMultSeqNumber);
            }
            if (nSeqSymbol > 0)
            {  // Sequential symbol strings exist (3 characters or more)
                nScore = parseInt(nScore - (nSeqSymbol * nMultSeqSymbol));
                sSeqSymbol = "- " + parseInt(nSeqSymbol * nMultSeqSymbol);
            }

            /* Determine if mandatory requirements have been met and set image indicators accordingly */
            var arrChars = [nLength, nAlphaUC, nAlphaLC, nNumber, nSymbol];
            var arrCharsIds = ["nLength", "nAlphaUC", "nAlphaLC", "nNumber", "nSymbol"];
            var arrCharsLen = arrChars.length;
            for (var c = 0; c < arrCharsLen; c++)
            {
                var oImg = $('div_' + arrCharsIds[c]);
                var oBonus = $(arrCharsIds[c] + 'Bonus');
                if (arrCharsIds[c] == "nLength") { var minVal = parseInt(nMinPwdLen - 1); } else { var minVal = 0; }
                if (arrChars[c] == parseInt(minVal + 1)) { nReqChar++; }
                else if (arrChars[c] > parseInt(minVal + 1)) { nReqChar++; }
                else { }
            }
            nRequirements = nReqChar;
            if (pwd.length >= nMinPwdLen) { var nMinReqChars = 3; } else { var nMinReqChars = 4; }
            if (nRequirements > nMinReqChars)
            {  // One or more required characters exist
                nScore = parseInt(nScore + (nRequirements * 2));
                sRequirements = "+ " + parseInt(nRequirements * 2);
            }

            return nScore;

        }
        else
        {
            return 0;
        }
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
    var initialPageButtonHide = function (step)
    {
        for (var i = 2; i <= step; i++)
        {
            $('#page' + i).hide();
        }
    };
    var nextForword = function (formId, index, isNext)
    {
        if (isNext)
        {
            $("#" + formId).steps("setStep", index);
            $('#page' + (index + 1)).show();
            $('#page' + index).hide();
        } else
        {
            $("#" + formId).steps("setStep", index);
            $('#page' + (index + 2)).hide();
            $('#page' + (index + 1)).show();
        }

    };
    var initialStep = function (formId, step)
    {
        $("#" + formId).steps({
            bodyTag: "fieldset",
            enablePagination: false
        });
        initialPageButtonHide(step);
        $.fn.steps.setStep = function (step)
        {

            var currentIndex = $(this).steps('getCurrentIndex');
            for (var i = 0; i < Math.abs(step - currentIndex); i++)
            {
                if (step > currentIndex)
                {
                    $(this).steps('next');
                }
                else
                {
                    $(this).steps('previous');
                }
            }
        };
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
        printMe: printMe,
        isNullOrEmpty: isNullOrEmpty,
        bootboxYesNo: bootboxYesNo,
        formateDate: formateDate,
        getMonthNameInShortForm: getMonthNameInShortForm,
        loadTable: loadTable,
        addRow2Table: addRow2Table,
        loadPieChart: loadPieChart,
        loadBarHierarchy: loadBarHierarchy,
        initializeFormValidation: initializeFormValidation,
        showCustomerModal: showCustomerModal,
        loadCustomers: loadCustomers,
        setVData: setVData,
        chkPass: chkPass,
        showLoder: showLoder,
        initialStep: initialStep,
        nextForword: nextForword
    };
}();