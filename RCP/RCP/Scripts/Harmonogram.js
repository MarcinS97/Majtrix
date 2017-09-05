/* -------------------------------------------------- NOWY HARMONOGRAM --------------------------------------------------- */
/*
    nie odpowiadam za ten kod - Skiddy
*/

temp = null;
DEBUG = false;


/* plugin do sortowania */
jQuery.fn.sortElements = (function () {

    var sort = [].sort;

    return function (comparator, getSortable) {

        getSortable = getSortable || function () { return this; };

        var placements = this.map(function () {

            var sortElement = getSortable.call(this),
                parentNode = sortElement.parentNode,
                nextSibling = parentNode.insertBefore(
                    document.createTextNode(''),
                    sortElement.nextSibling
                );
            return function () {
                if (parentNode === this) {
                    throw new Error(
                        "You can't sort elements if any one is a descendant of another."
                    );
                }
                parentNode.insertBefore(this, nextSibling);
                parentNode.removeChild(nextSibling);
            };
        });
        return sort.call(this, comparator).each(function (i) {
            placements[i].call(getSortable.call(this));
        });
    };

})();

/* plugin do ctrla */
$.ctrl = function (key, callback, args) {
    var isCtrl = false;
    $(document).keydown(function (e) {
        if (!args) args = []; // IE barks when args is null
        if (e.ctrlKey) isCtrl = true;
        if (e.keyCode == key.charCodeAt(0) && isCtrl) {
            callback.apply(this, args);
            return false;
        }
    }).keyup(function (e) {
        if (e.ctrlKey) isCtrl = false;
    });
};

function debug(s) {
    if (DEBUG)
        console.log(s);
}
function debugOk(s) {
    if (DEBUG)
        console.log('%c' + s, 'color: Green');
}
function debugError(s) {
    if (DEBUG)
        console.log('%c' + s, 'color: Red');
}

function PrepareHarmonogram() {
    var minRow, minCol, maxRow, maxCol, startRow, startCol, lastRow, lastCol;
    var rowDragging = false, colDragging = false, cellDragging = false, cellDraggingRight = false;
    var lastRow = null;
    var isCtrl = false, isShift = false;
    $(document).keydown(function (e) {
        if (e.ctrlKey) isCtrl = true;
        if (e.shiftKey) isShift = true;
    }).keyup(function (e) {
        isCtrl = false;
        isShift = false;
    });

    var handled = false;

    $(document)
        .mouseup(function () {
            rowDragging = false;
            colDragging = false;
            cellDragging = false;
            cellDraggingRight = false;
        });


    $(".col-opener").on("click", function (e) {
        $(".before").toggle();
        $(".sum").toggle();
        $(this).find("i").toggleClass("glyphicon-menu-left").toggleClass("glyphicon-menu-right");
        e.preventDefault();
        e.stopPropagation();
    });

    //$(".ddlKody").on("change", function (ev) {
    //    var that = $(this);
    //    var $divNewCode = that.parent().find(".divNewCode");
    //    var val = that.val();
    //    $divNewCode.toggleClass("hidden", val != -1);
    //    ev.stopPropagation();
    //});



    $('[data-toggle="tooltip"]').tooltip('destroy');
    $('[data-toggle="tooltip"]').tooltip();


    var table = $(".cntHarmonogram");
    $('.sortable th.sortable')
    .wrapInner('<span title="Sortuj"/>')
    .each(function () {
        var th = $(this),
            thIndex = th.index(),
            inverse = false;
        th.click(function () {
            table.find('td').filter(function () {

                return $(this).index() === thIndex;

            }).sortElements(function (a, b) {

                if ($.text([a]) == $.text([b]))
                    return 0;

                return $.text([a]) > $.text([b]) ?
                    inverse ? -1 : 1
                    : inverse ? 1 : -1;

            }, function () {

                // parentNode is the element we want to move
                return this.parentNode;

            });
            inverse = !inverse;
        });

    });

    schedule = {
        data: [],
        config: {
            autoSave: false,
            autoSaveDelay: 10000,
            //errorsPanelDelay:           3000,
            editable: false,
            undoBufferSize: 20,

            selectors: {
                container: $('.cntHarmonogram'),
                containerWrapper: $('.cntHarmonogramWrapper'),
                containerPage: $('.pgHarmonogram'),
                table: $('.cntHarmonogram .tbData'),
                viewStateContainer: $('.hidSchedule'),
                rows: $('.cntHarmonogram .tbData tr.data'),
                cells: $('.cntHarmonogram .tbData td.shift'),
                rowSelectors: $('.cntHarmonogram .row-select'),
                colSelectors: $('.cntHarmonogram th.col-select'),
                selectedRows: $('.cntHarmonogram tr.row-selected'),
                selectedCols: $('.cntHarmonogram th.col-selected'),
                allRowsToggler: $('.cntHarmonogram th.pracname a.pracname'),
                shifts: $('.cntZmianySelect .zmiana'),
                shiftsContainer: $('.cntZmianySelect'),
                postbackTriggers: $('input, a'),
                codes: $('.cntHarmonogram a.kod'),
                functions: $('.cntHarmonogram a.funk'),
                rightSummary: $('.right-summary'),
                mainShift: $('.main-shift'),
                cbFillFree: $('.cbFillFree'),
                shiftsToggler: $('.shifts-toggler'),
                errors: $('.hidErrors'),
                instantErrors: $('.hidInstantErrors'),
                allEmployees: $('.hidAllEmployees'),
                dateFrom: $('.hidDataOd'),
                dateTo: $('.hidDataDo'),
                RPN: $('.hidRPN'),
                timeMachine: $('.time-machine'),
                history: $('.history'),
                preprocessor: $('.hidApp'),
                errorsPanel: $('.errors-panel')
            }
        },
        init: function (config) {
            $('.tooltip').hide();
            debug('Initializing...');
            if (config && typeof (config) == 'object') {
                $.extend(schedule.config, config);
            }
            schedule.$container = schedule.config.selectors.container;
            schedule.$containerWrapper = schedule.config.selectors.containerWrapper;
            schedule.$containerPage = schedule.config.selectors.containerPage;

            schedule.editable = schedule.isEditable();

            schedule.$viewState = schedule.config.selectors.viewStateContainer;
            schedule.$rows = schedule.config.selectors.rows;
            schedule.$cells = schedule.config.selectors.cells;
            schedule.$rowSelectors = schedule.config.selectors.rowSelectors;
            schedule.$colSelectors = schedule.config.selectors.colSelectors;
            schedule.$table = schedule.config.selectors.table;
            schedule.$allRowsToggler = schedule.config.selectors.allRowsToggler;
            schedule.$shifts = schedule.config.selectors.shifts;
            schedule.$mainShift = schedule.config.selectors.mainShift;
            schedule.$shiftsToggler = schedule.config.selectors.shiftsToggler;
            schedule.$postbackTriggers = schedule.config.selectors.postbackTriggers;
            schedule.$selectedRows = schedule.config.selectors.selectedRows;
            schedule.$selectedCols = schedule.config.selectors.selectedCols;
            schedule.$codes = schedule.config.selectors.codes;
            schedule.$functions = schedule.config.selectors.functions;
            schedule.$rightSummary = schedule.config.selectors.rightSummary;
            schedule.$cbFillFree = schedule.config.selectors.cbFillFree;
            schedule.$errors = schedule.config.selectors.errors;
            schedule.$instantErrors = schedule.config.selectors.instantErrors;

            schedule.allEmployees = schedule.config.selectors.allEmployees.val();
            schedule.dateFrom = schedule.config.selectors.dateFrom.val();
            schedule.dateTo = schedule.config.selectors.dateTo.val();

            schedule.$RPN = schedule.config.selectors.RPN;
            schedule.RPN = schedule.getRPN();

            schedule.$timeMachine = schedule.config.selectors.timeMachine;
            schedule.$history = schedule.config.selectors.history;

            schedule.preprocessor = schedule.config.selectors.preprocessor.val();
            schedule.$errorsPanel = schedule.config.selectors.errorsPanel;


            if (schedule.editable) {
                schedule.prepareEdit();
            } else {
                schedule.preparePreview();
            }

            $(document).click(function (ev) {
                var target = $(ev.target);
                //if (target.is(('#content, .pgHarmonogram, .cntHarmonogramWrapper') || target.parents('.menu-box, .pagetitle, .navbar').length) && !target.is("input"))
                if (!target.is("input, a, select") && !target.parents(".cntHarmonogram").length && !target.parents(".modal").length)
                    schedule.deselectAll();
            });

            schedule.kodFiltered = false;
            schedule.funkFiltered = false;
            schedule.$codes.on("click", function (ev) {
                var value = $(this).data('value');
                if (schedule.kodFiltered) {
                    schedule.$table.find("tr:hidden").toggle(true);
                    schedule.kodFiltered = false;
                } else {
                    schedule.$table.find("a.kod:not([data-value='" + value + "'])").parents("tr").toggle(false);
                    schedule.kodFiltered = true;
                }
            });
            schedule.$functions.on("click", function (ev) {
                var value = $(this).data('value');
                if (schedule.funkFiltered) {
                    schedule.$table.find("tr:hidden").toggle(true);
                    schedule.funkFiltered = false;
                } else {
                    schedule.$table.find("a.funk:not([data-value='" + value + "'])").parents("tr").toggle(false);
                    schedule.funkFiltered = true;
                }
            });
            schedule.$rightSummary.on("click", function (ev) {
                schedule.onRightSummaryTogglerClick(ev);
            });

            schedule.$errorsPanel.on('click', function (ev) {
                var target = $(ev.target);
                if (!target.is('.panel-heading')
                    && !target.parents('.panel-heading').length
                    && !target.parents('.panel-body').length
                    && !target.is('#overlay')
                    && !target.is('a')) {
                    schedule.onErrorsPanelTogglerClick(ev);
                }
            });

            schedule.hidePopout(schedule.$rightSummary, 'no-anim');
            schedule.hidePopout(schedule.$errorsPanel, 'no-anim');
            //schedule.showPopout(schedule.$errorsPanel);
            /* time machine */

            schedule.$timeMachine.on('click', function (ev) {
                schedule.hideEmployeeHistory();
                schedule.showEmployeeHistory($(this));
                ev.stopPropagation();
            });

            $(window).click(function () {
                //Hide the menus if visible
                schedule.hideEmployeeHistory();
            });


            debugOk('Initialized');
        },
        prepareEdit: function () {
            handled = false;
            var data = [];
            if (schedule.isViewState()) {
                debug('Loading ViewState data...');
                data = schedule.getViewStateData();
                debug('ViewState data loaded');
            }
            else {
                debug('Loading QueryData...');
                data = schedule.getQueryData();
                debugOk('QueryData loaded');
            }
            debug(data);
            schedule.data = data;
            schedule.redraw();

            schedule.$cells
                .mousedown(schedule.onCellMouseDown)
                .mouseover(schedule.onCellMouseOver)
                .mouseup(schedule.onCellMouseUp);

            schedule.$rowSelectors
                .mousedown(schedule.onRowMouseDown)
                .mouseover(schedule.onRowMouseOver);

            schedule.$colSelectors
                .mousedown(schedule.onColMouseDown)
                .mouseover(schedule.onColMouseOver);

            schedule.$table.on('contextmenu', 'td.shift', function (e) { return false; });
            schedule.$allRowsToggler.on("click", schedule.toggleAllRows);
            schedule.$shifts.on('click', schedule.onShiftChanged);
            schedule.$postbackTriggers.on("click", schedule.onPostback);

            $.ctrl("C", function () {
                if (schedule.anyRowSelected()) {
                    schedule.copyRow();
                }
                else if (schedule.anyColSelected()) {
                    schedule.copyCol();
                } else {
                    //schedule.copyBlock();
                }
            });
            $.ctrl("V", function () {
                if (schedule.anyRowSelected()) {
                    schedule.pasteRow();
                }
                else if (schedule.anyColSelected()) {
                    schedule.pasteCol();
                }
                else {
                    //schedule.pasteBlock();
                }
                schedule.saveToBuffer();
            });
            $.ctrl("Z", function () {
                schedule.undo();
            });
            $.ctrl("Y", function () {
                schedule.redo();
            });
            $.ctrl("A", function () {
                schedule.toggleAllRows();
            });
            $.ctrl("K", function () {

                if (gol.running) {
                    clearInterval(gol.intervalId);
                    gol.running = false;
                } else {
                    gol.intervalId = setInterval(function () {
                        gol.newGeneration();
                    }, gol.delay);
                    gol.running = true;
                }
            });

            schedule.fillFree = schedule.$cbFillFree.prop('checked');
            schedule.$cbFillFree.on("click", function () {
                var checked = $(this).prop('checked');
                schedule.fillFree = checked;
            });
            if (schedule.config.autoSave) {
                schedule.setAutoSaveInterval();
            }
            schedule.changeTrigger();

            schedule.$shiftsToggler.hover(function () {
                var that = $(this);
                var id = that.attr('id');
                setTimeout(function () {
                    var hovered = that.parent().find("#" + id + ":hover").length;
                    if (hovered)
                        that.find("ul").show("fast");
                }, 1200);
            }, function (ev) {
                var $el = $(this);
                setTimeout(function () {
                    $el.find("ul").hide("fast");
                }, 500);
            });
            $(document).keydown(function (ev) {
                if (ev.keyCode == 46) {
                    if (schedule.anySelected()) {
                        schedule.clearShifts(); /* jak coś zaznaczyliśmy to czyścimy to */
                        schedule.changeTrigger();
                    }
                    else {
                        schedule.clearShift(row, col); /* jak klikneliśmy tylko to czyścimy jedna*/
                        schedule.changeTriggerRow(row);
                    }
                }
            });

            // search
            $(".tb-search").on("keyup", function () {
                var that = $(this);
                var value = that.val();
                schedule.searchEmployees(value);
            });

            $('.btn-search').attr('onclick', '');
            $('.btn-search').on('click', function () {
                schedule.searchEmployees($(".tb-search").val());
                return false;
            });

            $('.btn-search-clear').attr('onclick', '');
            $('.btn-search-clear').on('click', function () {
                $(".tb-search").val('');
                schedule.searchEmployees('');
                return false;
            });

            $('.btn-refresh-errors').on('click', schedule.onAutoSave);

            schedule.triggerErrorChange();

            schedule.getErrorsCount(function (count) {
                schedule.setErrorsCount(count);
            });
        },
        preparePreview: function () {
            schedule.$rowSelectors
               .mousedown(schedule.onRowMouseDown)
               .mouseover(schedule.onRowMouseOver);
            schedule.$postbackTriggers.on("click", schedule.onPostback);
            schedule.$allRowsToggler.on("click", schedule.onAllRowsSelected);
            schedule.$mainShift.popover();
            schedule.triggerErrorChange();
        },
        getViewStateData: function () {
            debug('Get ViewState Data...');
            var vs = schedule.$viewState.val();
            var data = JSON.parse(vs);
            var newData = [];
            for (var i = 0; i < data.length; i++) {
                var row = [];
                for (var j = 0 ; j < data[i].length; j++) {
                    var nel = schedule.getElementByIndex(i, j);
                    nel.attr("data-id", data[i][j].id);
                    row.push({
                        id: data[i][j].id,
                        sel: data[i][j].sel,
                        time: data[i][j].time,
                        el: nel
                    });
                }
                newData.push(row);
            }
            return newData;
        },
        getQueryData: function () {
            var newData = [];
            schedule.$rows.each(function () {
                var row = [];
                $(this).find(schedule.$cells).each(function () {
                    //var id = $(this).attr("data-id");
                    var id = $(this).data("id");
                    //var time = parseFloat($(this).attr("data-time"));
                    var time = parseFloat($(this).data("time"));
                    row.push({
                        id: id,
                        sel: false,
                        time: time,
                        el: $(this)
                    });
                });
                newData.push(row);
            });
            return newData;
        },
        getElementByIndex: function (i, j) {
            var $el = schedule.$table.find("tr[data-row-index='" + i + "']").find("td.shift[data-col-index='" + j + "']");
            return $el;
        },
        getRowByIndex: function (i) {
            //var $el = schedule.$table.find("tr[data-row-index='" + i + "']"); 
            //to zoptymalizowało o jakieś 500%
            var $el = schedule.$rows.eq(i);
            return $el;
        },
        getColByIndex: function (j) {
            var $el = schedule.$container.find("th[data-col-index='" + j + "']");
            return $el;
        },
        selectCell: function (i, j) {
            schedule.data[i][j].sel = true;
            schedule.data[i][j].el.toggleClass("cell-selected", true);
        },
        toggleCell: function (i, j) {
            schedule.data[i][j].sel = !schedule.data[i][j].sel;
            schedule.data[i][j].el.toggleClass("cell-selected");
        },
        selectCells: function (startRow, startCol, row, col) {
            if (schedule.selectedShift.sel) {
                for (var i = minRow; i <= maxRow; i++) {
                    for (var j = minCol; j <= maxCol; j++) {
                        schedule.clearShift(i, j);
                    }
                }


            } else {
                for (var i = minRow; i <= maxRow; i++) {
                    for (var j = minCol; j <= maxCol; j++) {
                        schedule.deselectCell(i, j);
                    }
                }
            }

            if (row <= startRow) {
                maxRow = startRow;
                minRow = row;
            }

            if (row >= startRow) {
                maxRow = row;
                minRow = startRow;
            }


            if (col <= startCol) {
                maxCol = startCol;
                minCol = col;
            }

            if (col >= startCol) {
                maxCol = col;
                minCol = startCol;
            }


            if (schedule.selectedShift.sel) {
                for (var i = minRow; i <= maxRow; i++) {
                    for (var j = minCol; j <= maxCol; j++) {
                        schedule.setShift(i, j);
                    }
                }
            }
            else {
                for (var i = minRow; i <= maxRow; i++) {
                    for (var j = minCol; j <= maxCol; j++) {
                        schedule.selectCell(i, j, "cell-selected");
                    }
                }
            }


        },
        selectRow: function (i) {
            var $tr = schedule.getRowByIndex(i);
            $tr.find(".row-select").toggleClass("row-selected", true);
            $tr.toggleClass("row-selected", true);
            if (schedule.editable) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    schedule.data[i][j].sel = true;
                    schedule.data[i][j].el.toggleClass("cell-selected", true);
                }
            }
            else {
                $tr.find("td").each(function () {
                    $(this).toggleClass("cell-selected");
                });
            }
        },
        toggleRow: function (i) {
            var $tr = schedule.getRowByIndex(i);
            $tr.find(".row-select").toggleClass("row-selected");
            $tr.toggleClass("row-selected");
            if (schedule.editable) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    schedule.data[i][j].sel = !schedule.data[i][j].sel;
                    schedule.data[i][j].el.toggleClass("cell-selected");
                }
            } else {
                $tr.find("td").each(function () {
                    //schedule.data[i][j].sel = !schedule.data[i][j].sel;
                    $(this).toggleClass("cell-selected");
                });
            }
        },
        toggleAllRows: function () {
            for (var i = 0; i < schedule.data.length; i++) {
                var $tr = schedule.getRowByIndex(i);
                if ($tr.is(':visible')) {
                    schedule.toggleRow(i);
                }
            }
        },
        selectRows: function (start, end) {
            var startRow = parseInt(start.attr("data-row-index"));
            var endRow = parseInt(end.attr("data-row-index"));
            var minRow, maxRow;

            if (startRow <= endRow) {
                minRow = startRow;
                maxRow = endRow;
            } else {
                minRow = endRow;
                maxRow = startRow;
            }

            for (var i = minRow; i <= maxRow; i++) {
                schedule.selectRow(i);
            }
        },
        selectCol: function (j) {
            var $th = schedule.getColByIndex(j); //$(".tbData th[data-col-index='" + j + "']");
            $th.toggleClass("col-selected", true);
            for (var i = 0; i < schedule.data.length; i++) {
                schedule.selectCell(i, j);
            }
        },
        selectCols: function (start, end) {
            var startCol = parseInt(start.attr("data-col-index"));
            var endCol = parseInt(end.attr("data-col-index"));
            var minCol, maxCol;

            if (startCol <= endCol) {
                minCol = startCol;
                maxCol = endCol;
            } else {
                minCol = endCol;
                maxCol = startCol;
            }

            for (var j = minCol; j <= maxCol; j++) {
                schedule.selectCol(j);
            }
        },
        toggleCol: function (j) {
            var $th = schedule.getColByIndex(j);
            $th.toggleClass("col-selected");
            for (var i = 0; i < schedule.data.length; i++) {
                schedule.toggleCell(i, j);
            }
        },
        toggleAllCols: function () {
            for (var j = 0; j < schedule.data[0].length; j++) {
                schedule.toggleCol(j);
            }
        },
        selectedShift: {
            id: null,
            color: '#ccc',
            name: '',
            time: 8,
            sel: false
        },
        setShifts: function () {
            for (var i = 0; i < schedule.data.length; i++) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.data[i][j].sel) {
                        schedule.setShift(i, j, false);
                    }
                }
            }
        },
        setShift: function (i, j, fill) {
            if ((!schedule.fillFree && !schedule.isDayFree(j)) || (schedule.fillFree) || fill)
                schedule.setDataByShift(i, j, schedule.selectedShift);
        },
        clearShift: function (i, j) {
            schedule.setData(i, j, null, "", "", null);
        },
        clearShifts: function () {
            for (var i = 0; i < schedule.data.length; i++) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.data[i][j].sel) {
                        schedule.setData(i, j, null, "", "", null);
                    }
                }
            }
        },
        isViewState: function () {
            var vs = schedule.$viewState.val();
            return !schedule.isEmpty(vs);
        },
        isEmpty: function (val) {
            return val === '' || val == null || val == undefined;
        },
        deselectCell: function (i, j) {
            schedule.data[i][j].sel = false;
            schedule.data[i][j].el.removeClass("row-selected").removeClass("col-selected").removeClass("cell-selected");
        },
        deselectRow: function (i) {
            var $tr = $(".tbData tr[data-row-index='" + i + "']");//parseInt(tr.attr("data-row-index"));

            $tr.find(".row-select").removeClass("row-selected");
            $tr.removeClass("row-selected");

            if (schedule.editable) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    schedule.deselectCell(i, j);
                }
            } else {
                $tr.find("td").each(function () {
                    $(this).removeClass("row-selected").removeClass("col-selected").removeClass("cell-selected");
                });
            }


        },
        deselectRows: function (minRow, maxRow) {
            for (var i = minRow; i <= maxRow; i++) {
                schedule.deselectRow(i);
            }
        },
        deselectCol: function (j) {
            var th = $(".tbData th[data-col-index='" + j + "']");
            th.removeClass("col-selected");

            for (var i = 0; i < schedule.data.length; i++) {
                schedule.deselectCell(i, j);
            }
        },
        deselectCols: function (minCol, maxCol) {
            for (var j = minCol; j <= maxCol; j++) {
                schedule.deselectCol(j);
            }
        },
        deselectAll: function () {
            $(".row-selected").each(function () {
                $(this).removeClass("row-selected");
            });

            $(".col-selected").each(function () {
                $(this).removeClass("col-selected");
            });

            $(".cell-selected").each(function () {
                $(this).removeClass("cell-selected");
            });


            for (var i = 0; i < schedule.data.length; i++) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    schedule.data[i][j].sel = false;

                }
            }
        },
        anySelected: function () {
            for (var i = 0; i < schedule.data.length; i++) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.data[i][j].sel)
                        return true;
                }
            }
            return false;

        },
        anyRowSelected: function () {
            return $("tr.row-selected").length > 0;
        },
        anyColSelected: function () {
            return $("th.col-selected").length > 0;
        },
        getFirstSelectedRowIndex: function () {
            var frow = $("tr.row-selected");
            return parseInt(frow.data("row-index"));
        },
        getFirstSelectedColIndex: function () {
            var fcol = $("th.col-selected");
            return parseInt(fcol.data("col-index"));
        },
        copyRow: function () {
            var row = schedule.getFirstSelectedRowIndex();
            schedule.rowClip = [];
            for (var j = 0; j < schedule.data[row].length; j++) {
                schedule.rowClip.push(schedule.data[row][j]);
            }
        },
        copyCol: function () {
            var col = schedule.getFirstSelectedColIndex();
            schedule.colClip = [];
            for (var i = 0; i < schedule.data.length; i++) {
                schedule.colClip.push(schedule.data[i][col]);
            }
        },
        copyBlock: function () {
            schedule.blockClip.minRow = null;
            schedule.blockClip.minCol = null;
            schedule.blockClip.maxRow = null;
            schedule.blockClip.maxCol = null;
            var x = 0, y = 0;
            var blockData = [];
            for (var i = 0; i < schedule.data.length; i++) {
                var row = [];
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.data[i][j].sel) {
                        if (schedule.blockClip.minRow == null && schedule.blockClip.minCol == null) {
                            schedule.blockClip.minRow = i;
                            schedule.blockClip.minCol = j;
                        }
                        schedule.blockClip.maxRow = i;
                        schedule.blockClip.maxCol = j;
                        row.push(schedule.data[i][j]);
                    }
                }
                if (row.length > 0)
                    blockData.push(row);
            }
            //console.log(blockData);
            schedule.blockClip.data = blockData;
            //console.log('min: ' + minRow + ' ' + minCol);
            //console.log('max: ' + maxRow + ' ' + maxCol);
        },
        rowClip: [],
        colClip: [],
        blockClip: {
            minRow: null, minCol: null, maxRow: null, maxCol: null,
        },
        pasteRow: function () {
            $("tr.row-selected").each(function () {
                var that = $(this);
                var i = parseInt(that.data("row-index"));
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.isEmpty(schedule.rowClip[j].id)) {
                        schedule.data[i][j].id = null;
                        schedule.clearShiftDataByElement(schedule.data[i][j].el);

                    } else {
                        var shift = schedule.getShiftById(schedule.rowClip[j].id);
                        schedule.data[i][j].id = schedule.rowClip[j].id;
                        var time = schedule.rowClip[j].time;
                        schedule.data[i][j].time = time;
                        schedule.setShiftDisplayByElement(schedule.data[i][j].el, shift.name, shift.color, time);
                    }
                }
            });
            schedule.changeTrigger();
        },
        pasteCol: function () {
            $("th.col-selected").each(function () {
                var that = $(this);
                var j = parseInt(that.data("col-index"));
                for (var i = 0 ; i < schedule.data.length; i++) {
                    if (schedule.isEmpty(schedule.colClip[i].id)) {
                        schedule.clearShiftDataByElement(schedule.data[i][j].el);
                    } else {
                        var shift = schedule.getShiftById(schedule.colClip[i].id);
                        schedule.data[i][j].id = schedule.colClip[i].id;
                        var time = schedule.colClip[i].time;
                        schedule.data[i][j].time = time;
                        schedule.setShiftDisplayByElement(schedule.data[i][j].el, shift.name, shift.color, time);
                    }
                }
            });
        },
        pasteBlock: function () {
            for (var i = 0; i < schedule.blockClip.data.length; i++) {
                for (var j = 0; j < schedule.blockClip.data[i].length; j++) {
                    if (schedule.isEmpty(schedule.blockClip.data[i][j].id)) {
                        schedule.data[minRow + i][minCol + j].id = null;
                        schedule.clearShiftDataByElement(schedule.data[minRow + i][minCol + j].el);

                    } else {
                        var shift = schedule.getShiftById(schedule.blockClip.data[i][j].id);
                        schedule.data[minRow + i][minRow + j].id = schedule.blockClip.data[i][j].id;
                        var time = schedule.blockClip.data[i][j].time;
                        schedule.data[minRow + i][minCol + j].time = time;
                        schedule.setShiftDisplayByElement(schedule.data[minRow + i][minCol + j].el, shift.name, shift.color, time);
                    }
                }
            }
            schedule.changeTrigger();
        },
        changeBuffer: [],
        saveToBuffer: function () {
            schedule.changeBuffer.push($.extend(true, [], schedule.data));
            if (schedule.changeBuffer.length > schedule.config.undoBufferSize) {
                schedule.changeBuffer.shift();
            }
            debug('Saving to buffer...');
            debug(schedule.changeBuffer);
        },
        undo: function () {
            if (schedule.changeBuffer.length > 1) {
                schedule.changeBuffer.splice(schedule.changeBuffer.length - 1, 1);
                schedule.data = $.extend(true, [], schedule.changeBuffer[schedule.changeBuffer.length - 1]);
            }
            debug('Undo...');
            debug(schedule.changeBuffer);
            schedule.redraw();

        },
        redo: function () {
            //TODO
        },
        redraw: function () {
            debug('Redraw...');
            for (var i = 0; i < schedule.data.length; i++) {
                for (var j = 0; j < schedule.data[i].length; j++) {
                    var shift = schedule.getShiftById(schedule.data[i][j].id);
                    if (schedule.isEmpty(shift.id) || isNaN(shift.id)) {
                        schedule.clearShiftDataByIndex(i, j);
                    } else {
                        var time = (schedule.data[i][j].time == 0) ? null : schedule.data[i][j].time;
                        schedule.setShiftDisplayByElement(schedule.data[i][j].el, shift.name, shift.color, /*shift.time*/time);
                    }
                    if (schedule.data[i][j].sel)
                        schedule.selectCell(i, j);
                }
            }
        },
        getSaveData: function () {
            var JSONArr = { data: [] };
            for (var i = 0; i < schedule.data.length; i++) {
                //var empId = $("tr[data-row-index='" + i + "'").attr("data-employee-id");
                var empId = schedule.$rows.eq(i).data("employee-id");
                for (var j = 0; j < schedule.data[i].length; j++) {
                    if (schedule.data[i][j].id != schedule.data[i][j].el.data("id")
                        || schedule.data[i][j].time !== schedule.data[i][j].el.data("time")) {
                        debug('Saving... ' + i + " " + j);
                        var date = $("th[data-col-index='" + j + "'").data("date");
                        var obj = {
                            ShiftId: schedule.data[i][j].id,
                            EmployeeId: empId,
                            Time: schedule.data[i][j].time,
                            Date: date
                        };
                        JSONArr.data.push(obj);
                    }
                }
            }
            return JSONArr;
        },
        save: function (successCallback) {
            var data = schedule.getSaveData();
            if (data.data.length > 0) {
                var encodedData = JSON.stringify(data);
                schedule.showAlert("Trwa zapisywanie", "alert-info");
                showLoading($('.cntHarmonogram'));
                $.ajax({
                    type: "POST",
                    url: "Harmonogram3.aspx/SaveSchedule",
                    data: encodedData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function () {
                        hideLoading($('.cntHarmonogram'));
                        schedule.saveSuccess(data);
                        successCallback();

                    },
                    error: function (response) {
                        hideLoading($('.cntHarmonogram'));
                        debugError('Saving error!!!');
                        debug(response);
                        schedule.saveFailure();
                        setTimeout(function () {
                            $("#alertdiv").remove();
                        }, 2000);
                    }
                });
            } else {
                successCallback();
            }
        },
        saveToViewState: function () {
            var data = schedule.data;
            var newData = [];
            for (var i = 0; i < data.length; i++) {
                var row = [];
                for (var j = 0; j < data[i].length; j++) {
                    row.push({
                        id: data[i][j].id,
                        sel: data[i][j].sel,
                        time: data[i][j].time
                    });
                }
                newData.push(row);
            }
            var str = JSON.stringify(newData);
            debug("save to viewstate");
            $(".hidSchedule").val(str);
            schedule.saveSelectedRowsToViewState();
            // new
            schedule.saveAllRowsToViewState();
        },
        saveSelectedRowsToViewState: function () {
            var employeesStr = '';
            schedule.$table.find("tr.row-selected").each(function () {
                employeesStr += $(this).data("employee-id") + ";";
            });
            $('.hidSelectedEmployees').val(employeesStr);
        },
        saveAllRowsToViewState: function () {
            var employeesStr = '';
            schedule.$table.find("tr:visible").each(function () {
                employeesStr += $(this).data("employee-id") + ";";
            });
            $('.hidAllEmployees').val(employeesStr);
        },
        saveSuccess: function (savedData) {
            for (var x = 0; x < savedData.data.length; x++) {
                var i = $("tr[data-employee-id='" + savedData.data[x].EmployeeId + "'").data("row-index");
                var j = $("th[data-date='" + savedData.data[x].Date + "'").data("col-index");
                var shift = savedData.data[x].ShiftId;
                var time = savedData.data[x].Time;
                schedule.data[i][j].el.data("id", shift);
                schedule.data[i][j].el.data("time", time);
            }
            schedule.showAlert("Zapisano", "alert-success");
        },
        saveFailure: function () {
            schedule.showAlert("Błąd zapisu!", "alert-danger");
        },
        showAlert: function showalert(message, alerttype) { //"alert-error","alert-success","alert-info"
            var icon = "";
            if (alerttype == "alert-info") {
                icon = "<i class='fa fa-cog fa-spin'></i>"
            } else if (alerttype == "alert-success") {
                icon = "<i class='fa fa-check'></i>";
            } else {
                icon = "<i class='fa fa-exclamation-circle'></i>"
            }
            $('#alert_placeholder').html('<div id="alertdiv" class="alert ' + alerttype + '">' + icon + '<span>' + message + '</span></div>')
        },
        changeTrigger: function () {
            debug("Change trigger...");
            schedule.saveToBuffer();
            schedule.calculate();
        },
        changeTriggerRow: function (i) {
            schedule.saveToBuffer();
            schedule.calculateRow(i);
            if (schedule.isRightSummaryVisible())
                schedule.calculateRightSummary();
        },
        calculate: function () {
            for (var i = 0; i < schedule.data.length; i++) {
                schedule.calculateRow(i);
            }
            if (schedule.isRightSummaryVisible())
                schedule.calculateRightSummary();
        },
        calculateRow: function (i) {
            var rsum = 0, psum = 0, nsum = 0, ndsum = 0, sum = 0;
            for (var j = 0; j < schedule.data[i].length; j++) {
                if (schedule.data[i][j].id == schedule.RPN.R)
                    rsum++;
                if (schedule.data[i][j].id == schedule.RPN.P)
                    psum++;
                if (schedule.data[i][j].id == schedule.RPN.N)
                    nsum++;
                if (schedule.data[i][j].el.attr("class").indexOf("niedziela") > -1 && schedule.data[i][j].id != null && schedule.data[i][j].id != "")
                    ndsum++;
                if (!schedule.isEmpty(schedule.data[i][j].id))
                    sum += parseFloat(schedule.data[i][j].time);


                var $tr = schedule.getRowByIndex(i);
                $tr.find("td.rsum").text(rsum);
                $tr.find("td.psum").text(psum);
                $tr.find("td.nsum").text(nsum);
                $tr.find("td.ndsum").text(ndsum);

                var $tdNom = $tr.find("td.czasnom");
                var $tdCzasACtual = $tr.find("td.czasactual");
                var nom = parseInt($tdNom.attr("data-value"));

                $tdCzasACtual.text(sum);

                if (nom == sum && nom != 0) {
                    $tdCzasACtual.addClass('all-good');
                    $tdCzasACtual.removeClass('all-wrong all-quite');
                } else if (sum < nom && nom != 0) {
                    $tdCzasACtual.addClass('all-wrong');
                    $tdCzasACtual.removeClass('all-good all-quite');
                } else {
                    $tdCzasACtual.addClass('all-quite');
                    $tdCzasACtual.removeClass('all-good all-wrong');
                }


            }
        },
        onCellMouseDown: function (ev) {
            var $self = $(this);
            var row = parseInt($self.parent("tr").attr("data-row-index"));
            var col = parseInt($self.attr("data-col-index"));

            if (ev.which == 3) { // prawy
                cellDragging = false;
                cellDraggingRight = true;

                if (schedule.anySelected()) {
                    schedule.clearShifts(); /* jak coś zaznaczyliśmy to czyścimy to */
                    schedule.changeTrigger();
                }
                else {
                    schedule.clearShift(row, col); /* jak klikneliśmy tylko to czyścimy jedna*/
                    schedule.changeTriggerRow(row);
                }
            }
            else {
                if (isShift) {
                    schedule.deselectAll();
                } else {

                    cellDragging = true;
                    cellDraggingRight = false;


                    minRow = maxRow = startRow = row;
                    minCol = maxCol = startCol = col;

                    if (schedule.selectedShift.sel) {
                        schedule.setShift(row, col, true);

                    } else {
                        if (!isCtrl)
                            schedule.selectCell(row, col);
                        else
                            schedule.toggleCell(row, col);
                    }

                    if (!isCtrl)
                        schedule.deselectAll();

                    lastRow = row;
                    lastCol = col;
                }
            }
            return false;
        },
        onCellMouseOver: function (ev) {
            var $self = $(this);
            var row = parseInt($self.parent("tr").data("row-index"));
            var col = parseInt($self.data("col-index"));

            if (schedule.preprocessor.toLowerCase() == 'keeeper')
                cellDragging = false;

            if (cellDraggingRight) {
                schedule.clearShift(row, col); /* jak jezdzimy z wcisnietym prawym to czyscimy */
            }
            else if (cellDragging) {
                if (schedule.selectedShift.sel) {
                    for (var i = minRow; i <= maxRow; i++) {
                        for (var j = minCol; j <= maxCol; j++) {
                            schedule.clearShift(i, j);
                        }
                    }


                } else {
                    for (var i = minRow; i <= maxRow; i++) {
                        for (var j = minCol; j <= maxCol; j++) {
                            schedule.deselectCell(i, j);
                        }
                    }
                }

                if (row <= startRow) {
                    maxRow = startRow;
                    minRow = row;
                }

                if (row >= startRow) {
                    maxRow = row;
                    minRow = startRow;
                }


                if (col <= startCol) {
                    maxCol = startCol;
                    minCol = col;
                }

                if (col >= startCol) {
                    maxCol = col;
                    minCol = startCol;
                }


                if (schedule.selectedShift.sel) {

                    for (var i = minRow; i <= maxRow; i++) {
                        for (var j = minCol; j <= maxCol; j++) {
                            schedule.setShift(i, j);
                        }
                    }
                }
                else {
                    for (var i = minRow; i <= maxRow; i++) {
                        for (var j = minCol; j <= maxCol; j++) {
                            schedule.selectCell(i, j, "cell-selected");
                        }
                    }
                }
            }

        },
        onCellMouseUp: function (ev) {
            if (schedule.selectedShift.sel) {
                schedule.changeTrigger();
            }
        },
        onRowMouseDown: function (ev) {
            //console.log(schedule.preprocessor);
            if (schedule.preprocessor.toLowerCase() == 'keeeper') {
                rowDragging = false;
            }
            else {
                rowDragging = true;
            }
            //rowDragging = false;//true;
            var row = $(this).parent("tr");
            var rowIndex = parseInt(row.data("row-index"));

            if (isShift && lastRow != null) {
                var lastRowIndex = parseInt(lastRow.data("row-index"));
                schedule.deselectAll();
                schedule.selectRows(lastRow, row);
            }
            else {
                if (!isCtrl)
                    schedule.deselectAll();

                schedule.toggleRow(rowIndex);
                lastRow = row;
            }

            minRow = maxRow = startRow = rowIndex;//parseInt(row.attr("data-row-index"));
            return false;

        },
        onRowMouseOver: function (ev) {
            var row = parseInt($(this).parent("tr").data("row-index"));
            if (rowDragging) {
                lastRow = $(this).parents("tr");

                schedule.deselectRows(minRow, maxRow);

                if (row <= startRow) {
                    maxRow = startRow;
                    minRow = row;
                }

                if (row >= startRow) {
                    maxRow = row;
                    minRow = startRow;
                }

                for (var i = minRow; i <= maxRow; i++) {
                    schedule.selectRow(i);
                }
            }
        },
        onColMouseDown: function (ev) {
            colDragging = true;
            var col = $(this);
            var colIndex = parseInt($(this).data("col-index"));


            if (isShift && lastCol != null) {
                var lasColIndex = parseInt(lastCol.data("col-index"));
                schedule.deselectAll();
                schedule.selectCols(lastCol, col);
            } else {
                if (!isCtrl)
                    schedule.deselectAll();

                schedule.toggleCol(colIndex);
                lastCol = col;
            }

            minCol = maxCol = startCol = colIndex;
            return false;
        },
        onColMouseOver: function (ev) {
            var col = parseInt($(this).data("col-index"));
            if (colDragging) {
                lastCol = $(this);
                schedule.deselectCols(minCol, maxCol);

                if (col <= startCol) {
                    maxCol = startCol;
                    minCol = col;
                }

                if (col >= startCol) {
                    maxCol = col;
                    minCol = startCol;
                }

                for (var i = minCol; i <= maxCol; i++) {
                    schedule.selectCol(i);
                }
            }
        },
        onPostback: function (ev) {
            var oncl = $(this).attr("onclick");
            var href = $(this).attr("href");

            //debug("Postback..." + handled);
            //debug("oncl: " + oncl);
            //debug("href: " + href);
            if ((oncl != undefined && oncl.indexOf("PostBack") != -1) || (href != undefined && href.indexOf("PostBack") != -1)) {
                schedule.prePostBackCleanup($(this));
                var id = $(this).attr("id");
                if (!handled) {
                    if (schedule.editable) {
                        schedule.save(function () {
                            debug("save viewstate callback");
                            schedule.saveToViewState();
                            setTimeout(function () {
                                $("#alertdiv").remove();
                            }, 2000);
                            handled = true;
                            debug("click on " + id);
                            if (oncl != undefined)
                                $("#" + id).trigger('click');
                            else
                                eval(href);

                            return true;
                        });
                    } else {
                        schedule.saveSelectedRowsToViewState();
                        schedule.saveAllRowsToViewState();
                        handled = true;
                        debug("click on " + id);
                        if (oncl != undefined) {
                            $("#" + id).trigger('click');
                        }
                        else {
                            eval(href);
                        }
                    }
                    ev.preventDefault();
                    //}
                    handled = false;
                    return true;
                } else {
                    handled = false;
                    return true;
                }
            }
        },
        onShiftChanged: function (ev) {
            var id = $(this).data("id");
            var color = $(this).data("color");
            var name = $(this).data("name");
            var time = $(this).data("time");

            schedule.selectedShift.color = color;
            schedule.selectedShift.name = name;
            schedule.selectedShift.time = time;
            schedule.selectedShift.el = $(this);

            if (schedule.anySelected()) {
                schedule.selectedShift.id = id;
                schedule.setShifts();
                schedule.changeTrigger();
            } else {
                $(".cntZmianySelect .zmiana").each(function () {
                    $(this).removeClass("sit");
                });
                if (id == schedule.selectedShift.id && time == schedule.selectedShift.time) {
                    if (schedule.selectedShift.sel) {
                        $(this).removeClass("sit");
                        schedule.selectedShift.sel = false;
                        schedule.setMainShiftData($(this).parents(".shifts-toggler").find(".main-shift"), time, false);
                    }
                    else {
                        $(this).toggleClass("sit");
                        schedule.selectedShift.sel = true;
                        schedule.setMainShiftData($(this).parents(".shifts-toggler").find(".main-shift"), time, true);
                    }
                } else {
                    $(this).toggleClass("sit");
                    schedule.selectedShift.sel = true;
                    schedule.setMainShiftData($(this).parents(".shifts-toggler").find(".main-shift"), time, true);
                }
                schedule.selectedShift.id = id;
            }
        },
        setMainShiftData: function ($el, time, b) {
            $el.attr("data-time", time);
            if (time > 0)
                $el.find(".no").text(time);
            $el.toggleClass("sit", b);
        },
        onRowSelected: function (ev) {
            var tr = $(this).parent("tr");
            tr.toggleClass("row-selected");
            tr.find("td").each(function () {
                $(this).toggleClass("row-selected");
            });
        },
        onAllRowsSelected: function () {
            //alert('asd');
            schedule.$rows.each(function () {
                var tr = $(this);//.parent("tr");
                if (tr.is(':visible')) {
                    tr.toggleClass("row-selected");
                    tr.find("td").each(function () {
                        $(this).toggleClass("row-selected");
                    });
                }
            });
        },
        onAutoSave: function (ev) {
            debug('Autosaving...');
            if (!(schedule.saving || false)) {
                schedule.saving = true;
                schedule.save(schedule.onAutoSaved);
            }
        },
        onAutoSaved: function () {
            debugOk('Autosaved');
            setTimeout(function () {
                $("#alertdiv").remove();
            }, 2000);
            schedule.saving = false;
            schedule.setAutoSaveInterval();
            schedule.getErrors();
            if (schedule.isErrorsPanelVisible()) {
                schedule.getInstantErrors(function (data) {
                    schedule.loadErrorsPanel(data);
                });
            } else {
            }
            schedule.getErrorsCount(function (count) {
                schedule.setErrorsCount(count);
            });
            //schedule.calculate();
        },
        getShiftById: function (id) {
            var $found = schedule.$shifts.filter("[data-id='" + id + "']"); // tu był find
            return {
                id: parseInt($found.data("id")),
                color: $found.data("color"),
                name: $found.data("name")
            }
        },
        setDataByShift: function (i, j, shift) {
            schedule.data[i][j].id = shift.id;
            schedule.data[i][j].time = shift.time;
            schedule.setShiftDisplayByIndex(i, j, shift.name, shift.color, shift.time);
        },
        setData: function (i, j, id, name, color, time) {
            schedule.data[i][j].id = id;
            schedule.data[i][j].time = time;
            schedule.setShiftDisplayByIndex(i, j, name, color, time);
        },
        isDayFree: function (j) {
            var day = schedule.getColByIndex(j);
            var free = !schedule.isEmpty(day.data("type"));
            //debug('isDayFree...');
            //debug.log('type: ' + day.data('type'));
            //debug(day);
            //debug(free);
            return free;
        },
        calculateRightSummary: function () {
            showLoading(schedule.$rightSummary.find('.tbSummary'));
            for (var j = 0; j < schedule.data[0].length; j++) {
                var rsum = 0, r2sum = 0, psum = 0, p2sum = 0, nsum = 0, n2sum = 0;
                var $tr = schedule.$rightSummary.find("tr[data-row-index='" + j + "']");

                for (var i = 0; i < schedule.data.length; i++) {
                    var $scheduleRow = schedule.getRowByIndex(i);
                    var rodzaj = $scheduleRow.data('rodzaj');
                    var isFunk = !schedule.isEmpty(rodzaj);
                    if (schedule.data[i][j].id == schedule.RPN.R) {
                        rsum++;
                        if (!isFunk)
                            r2sum++;
                    }
                    if (schedule.data[i][j].id == schedule.RPN.P) {
                        psum++;
                        if (!isFunk)
                            p2sum++;
                    }
                    if (schedule.data[i][j].id == schedule.RPN.N) {
                        nsum++;
                        if (!isFunk)
                            n2sum++;
                    }
                }


                var oldR = parseInt($tr.find(".rsum").data('value'));
                var oldP = parseInt($tr.find(".psum").data('value'));
                var oldN = parseInt($tr.find(".nsum").data('value'));

                var oldR2 = parseInt($tr.find(".r2sum").data('value'));
                var oldP2 = parseInt($tr.find(".p2sum").data('value'));
                var oldN2 = parseInt($tr.find(".n2sum").data('value'));

                //console.log(oldN2 + ' ' + nsum);


                $tr.find("td.rsum").text(oldR + rsum);
                $tr.find("td.psum").text(oldP + psum);
                $tr.find("td.nsum").text(oldN + nsum);

                $tr.find("td.r2sum").text(oldR2 + r2sum);
                $tr.find("td.p2sum").text(oldP2 + p2sum);
                $tr.find("td.n2sum").text(oldN2 + n2sum);
            }

            hideLoading(schedule.$rightSummary.find('.tbSummary'));
        },
        isRightSummaryVisible: function () {
            return !schedule.$rightSummary.hasClass('h420');
        },
        isErrorsPanelVisible: function () {
            return !schedule.$errorsPanel.hasClass('h420');
        },
        showPopout: function (el) {
            //if (schedule.editable)
            //    schedule.calculateRightSummary();
            //el.animate({
            //    right: 0,
            //    specialEasing: {
            //        width: "linear",
            //        height: "easeOutBounce"
            //    }
            //}, 'slow', function () {
            //});
            el.css('right', 0);
            el.css('z-index', 10000);
            el.removeClass('h420');
        },
        hidePopout: function (el, anim) {
            var w = el./*find('table').*/width();
            el.css('right', -w);
            el.css('z-index', 9999);
            el.addClass('h420');
        },
        onTogglerClick: function (el, ev, callback, hideCallback) {
            if (el.hasClass('h420')) {
                schedule.showPopout(el);
                if (schedule.editable) {
                    callback();
                }
            }
            else {
                schedule.hidePopout(el);
                if (hideCallback)
                    hideCallback();
            }
            ev.stopPropagation();
        },
        onRightSummaryTogglerClick: function (ev) {
            schedule.onTogglerClick(schedule.$rightSummary, ev, function () {
                schedule.calculateRightSummary();
            });
        },
        onErrorsPanelTogglerClick: function (ev) {
            schedule.onTogglerClick(schedule.$errorsPanel, ev, function () {
                //console.log('load instant errors');
                schedule.getInstantErrors(function (data) {
                    schedule.loadErrorsPanel(data);
                });

                //schedule.errorsPanelInterval = setInterval(function () {
                //    schedule.getInstantErrors(function (data) {
                //        schedule.loadErrorsPanel(data);
                //    });
                //}, schedule.config.errorsPanelDelay);


            }, function () {
                clearInterval(schedule.errorsPanelInterval);
            });
        },
        setShiftName: function ($el, name) {
            $el.find(".name").text(name);
        },
        setShiftColor: function ($el, color) {
            $el.css("background", color);
        },
        setShiftTime: function ($el, time) {
            if (time > 0)
                $el.find('.time').text(time);
            else
                $el.find('.time').text('');
        },
        setShiftDisplayByElement: function ($el, name, color, time) {
            schedule.setShiftName($el, name);
            schedule.setShiftColor($el, color);
            schedule.setShiftTime($el, time);
        },
        setShiftDisplayByIndex: function (i, j, name, color, time) {
            var $el = schedule.getElementByIndex(i, j);
            schedule.setShiftDisplayByElement($el, name, color, time);
        },
        clearShiftDataByElement: function ($el) {
            schedule.setShiftDisplayByElement($el, "", "", "");
        },
        clearShiftDataByIndex: function (i, j) {
            var $el = schedule.getElementByIndex(i, j);
            schedule.setShiftDisplayByElement($el, "", "", "");
        },
        prePostBackCleanup: function (el) {
            if (el.hasClass('keep-as')) {

            } else {
                clearInterval(schedule.autoSaveInterval);
            }
        },
        setAutoSaveInterval: function () {
            clearInterval(schedule.autoSaveInterval);
            schedule.autoSaveInterval = setInterval(schedule.onAutoSave, schedule.config.autoSaveDelay);
        },
        isEditable: function () {
            return schedule.$container.data("editable") == "True";
        },
        triggerErrorChange: function () {
            schedule.clearErrors();
            schedule.setErrors(schedule.$errors.val());
        },
        clearErrors: function () {
            schedule.$cells.each(function () {
                $(this).removeClass('shift-error')
                $(this).attr('title', '');
                //$(this).tooltip('disable');
            });
        },
        setErrors: function (errors) {
            if (!schedule.isEmpty(errors)) {
                var errorsArr = [];
                errorsArr = JSON.parse(errors);
                for (var i = 0; i < errorsArr.length; i++) {
                    var id = errorsArr[i].id;
                    var date = errorsArr[i].date;
                    var $th = $(".tbHeader").find("th.shift[data-date='" + date + "']");
                    var index = $th.attr("data-col-index");
                    var $td = $(".tbData tr[data-employee-id='" + id + "']").find("td.shift[data-col-index='" + index + "']");
                    $td.toggleClass("shift-error");
                    $td.attr('title', 'Błąd!');
                }
            }
        },
        getErrors: function () {
            debug('Loading instant errors...');
            $.ajax({
                type: "POST",
                url: "Harmonogram3.aspx/GetErrors",
                data: '{"emp" : ' + '"' + schedule.allEmployees + '"' + ', "dataOd" :' + '"' + schedule.dateFrom + '", "dataDo" : "' + schedule.dateTo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    debugOk('Instant errors loaded');
                    debug(response);
                    schedule.clearErrors();
                    if (!schedule.isEmpty(response.d)) {
                        schedule.$instantErrors.val(response.d);
                        schedule.setErrors(response.d);
                        //schedule.triggerErrorChange();
                    }
                },
                error: function (response) {
                    debug('Error loading instant errors!!!');
                    debug(response);
                }
            });
        },
        getInstantErrors: function (callback) {
            debug('Loading instant errors...');
            var $t = $('.errors-panel-body').parent();
            //if($t.parent().find('#overlay').length <= 0)
            showLoading($t);
            $.ajax({
                type: "POST",
                url: "Harmonogram3.aspx/GetInstantErrors",
                data: '{"emp" : ' + '"' + schedule.allEmployees + '"' + ', "dataOd" :' + '"' + schedule.dateFrom + '", "dataDo" : "' + schedule.dateTo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    debugOk('Instant errors loaded');
                    debug(response);
                    //$('#overlay').fadeOut();
                    hideLoading($t);
                    callback(JSON.parse(response.d));
                },
                error: function (response) {
                    hideLoading($t);
                    debug('Error loading instant errors!!!');
                    debug(response);
                }
            });
        },
        getRPN: function () {
            var rpnRaw = schedule.$RPN.val();
            var rpnArr = rpnRaw.split(';');
            return {
                R: rpnArr[0],
                P: rpnArr[1],
                N: rpnArr[2]
            }
        },
        showEmployeeHistory: function (that) {
            var $parentTr = that.parents('tr');
            var empId = $parentTr.data('employee-id');
            if (empId) {
                schedule.getEmployeeHistory(empId, schedule.dateFrom, schedule.dateTo, function (data) {
                    schedule.temp = data;
                    for (var i = 0; i < data.length; i++) {
                        var row = "";
                        var colspan = 1;
                        if (schedule.preprocessor.toLowerCase() == 'keeeper') {
                            colspan = 3;
                        }


                        row =
                        "<tr class='it data history'>" +
                            "<td colspan='" + colspan + "' class='funk'>" +
                                "<label>" +
                                    data[i].date +
                                "</label>" +
                            "</td>";

                        data[i].shifts.sort(function (a, b) {
                            return a.day - b.day;
                        });
                        for (var j = 0; j < data[i].shifts.length; j++) {
                            row += "<td class='shift' style='background-color: " + data[i].shifts[j].color + "'>";
                            row += "<span class='name'>" + data[i].shifts[j].name + "</span>";
                            row += "<span class='no'>" + data[i].shifts[j].time + "</span>";

                            row += "</td>";
                        }
                        row += "<td class='firstsum' colspan='5'></td>";
                        row += "</tr>";
                        $parentTr.after(row);
                    }

                });
            }
        },
        getEmployeeHistory: function (empId, dateFrom, dateTo, successCallback) {

            var params = { "empId": empId, "dateFrom": dateFrom, "dateTo": dateTo };

            $.ajax({
                type: "POST",
                url: "Harmonogram3.aspx/GetHistory",//?empId=" + JSON.stringify(empId) + "&dateFrom=" + JSON.stringify(dateFrom) + "&dateTo=" + JSON.stringify(dateTo),
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //console.log(data.d);
                    //schedule.saveSuccess(data);
                    //successCallback();
                    successCallback(JSON.parse(data.d));
                },
                error: function (response) {
                    debugError('Getting history error!!!');
                    debug(response);
                    //schedule.saveFailure();
                    //setTimeout(function () {
                    //    $("#alertdiv").remove();
                    //}, 2000);
                }
            });
        },
        hideEmployeeHistory: function () {
            schedule.$container.find('.history').remove();
        },
        searchEmployees: function (empToSearch) {
            var rows = schedule.$rows;
            if (empToSearch.length > 0) {
                rows.hide();
                rows.filter(function () {
                    var that = $(this);
                    var pracNameLabel = that.find(".pracname label.name");
                    if (pracNameLabel.is(":icontains('" + empToSearch.trim() + "')"))
                        return true;
                    return false;
                }).show();
            }
            else {
                rows.show();
            }
        },
        loadErrorsPanel: function (data) {
            var $content = $('.errors-panel-body');

            $content.html('');
            if (data.length > 0) {
                var ul = $('<ul/>').addClass('list-group');
                for (var i = 0; i < data.length; i++) {
                    var text = data[i];
                    var li = $('<li/>').addClass('list-group-item').html(text);
                    ul.append(li);
                    $content.append(ul);
                }
            } else {
                $content.html("<div class='no-errors'>Plan pracy poprawny</div>");
            }
        },
        getErrorsCount: function (callback) {
            debug('Loading instant errors count...');
            //var $t = $('.errors-panel-body');
            //$('#overlay').fadeIn();
            //showLoading($('.error-panel-trigger'));
            $.ajax({
                type: "POST",
                url: "Harmonogram3.aspx/GetErrorsCount",
                data: '{"emp" : ' + '"' + schedule.allEmployees + '"' + ', "dataOd" :' + '"' + schedule.dateFrom + '", "dataDo" : "' + schedule.dateTo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    debugOk('Instant errors count loaded');
                    debug(response);

                    //hideLoading($('.error-panel-trigger'));
                    //$('#overlay').fadeOut();
                    //console.log(response);
                    var count = parseInt(response.d);
                    callback(count);
                },
                error: function (response) {
                    debug('Error loading instant errors!!!');
                    debug(response);
                }
            });
        },
        setErrorsCount: function (count) {
            if (count > 0) {
                $('.error-panel-trigger').addClass('number error-trigger-blink').attr('title', 'Ilość błędow: ' + count);
                $('.error-panel-trigger').html(count);
                $('.btn-export-errors').fadeIn();
            } else {
                $('.error-panel-trigger').removeClass('number error-trigger-blink').attr('title', 'Plan pracy poprawny');
                $('.error-panel-trigger').html("<i class='fa fa-check'></i>");
                $('.btn-export-errors').fadeOut();
            }
        },
        onRefreshErrorsButtonClick: function () {
            //console.log(1);
            if (!schedule.saving)
                schedule.save();
        }
    };

    schedule.init();


    gol = {
        buffer: [],
        newGeneration: function () {

            gol.buffer = [];

            for (var i = 0; i < schedule.data.length; i++) {
                var row = [];

                for (var j = 0; j < schedule.data[i].length; j++) {
                    var neighbours = gol.countNeighbours(i, j);
                    if (neighbours > 0)
                        console.log(neighbours);
                    var alive = !gol.isEmpty(schedule.data[i][j].id);
                    var newObj = schedule.data[i][j];

                    if (alive && (neighbours < 2 || neighbours > 3)) {
                        row.push({
                            id: null,
                            sel: false,
                            time: null,
                            el: schedule.data[i][j].el
                        });
                    }
                        /*else if (neighbours == 3) {
                            row.push({
                                id: 191,
                                sel: false,
                                color: "#000",
                                el: schedule.data[i][j].el,
                                name: "X"
                            });
                        }*/
                    else {
                        console.log('else');
                        row.push({
                            id: 189,
                            sel: false,
                            time: schedule.data[i][j].time,
                            //color: schedule.data[i][j].color,
                            el: schedule.data[i][j].el//,
                            //name: schedule.data[i][j].name
                        });
                    }
                }
                gol.buffer.push(row);
            }
            gol.print(schedule.data);
            gol.print(gol.buffer);
            schedule.data = $.extend(true, [], gol.buffer);
            gol.print(schedule.data);
            schedule.redraw();
        },
        countNeighbours: function (i, j) {
            var n = 0;

            var xlen = schedule.data[0].length - 1;
            var ylen = schedule.data.length - 1;

            if (i > 0 && j > 0) {
                if (!gol.isEmpty(schedule.data[i - 1][j - 1].id)) {
                    ++n;
                }
            }

            if (i > 0) {
                if (!gol.isEmpty(schedule.data[i - 1][j].id)) {
                    ++n;
                }
            }

            if (i > 0 && j < xlen) {
                if (!gol.isEmpty(schedule.data[i - 1][j + 1].id)) {
                    ++n;
                }
            }

            if (j < xlen) {
                if (!gol.isEmpty(schedule.data[i][j + 1].id)) {
                    ++n;
                }
            }

            if (i < ylen && j < xlen) {
                if (!gol.isEmpty(schedule.data[i + 1][j + 1].id)) {
                    ++n;
                }
            }

            if (i < ylen) {
                if (!gol.isEmpty(schedule.data[i + 1][j].id)) {
                    ++n;
                }
            }

            if (i < ylen && j > 0) {
                if (!gol.isEmpty(schedule.data[i + 1][j - 1].id)) {
                    ++n;
                }
            }

            if (j > 0) {
                if (!gol.isEmpty(schedule.data[i][j - 1].id)) {
                    ++n;
                }
            }
            return n;
        },
        isEmpty: function (obj) {
            return obj == "" || obj == null || obj == undefined;
        },
        print: function (arr) {
            var output = "";
            for (var i = 0; i < arr.length; i++) {
                for (var j = 0; j < arr[i].length; j++) {
                    var a = gol.isEmpty(arr[i][j].id) ? "0" : "1";
                    output += a
                }
                output += "\n";
            }

            alert(output);

        },
        delay: 30000
    }

}

function showLoading(el) {
    //console.log(el);
    var overlay = $('<div/>').attr('id', 'overlay');


    //var parent = $('<div/>').attr('id', 'circularG').addClass('img-load').html("<i class='fa fa-spin fa-spinner'></i>");

    var width = el.width();
    var height = el.height();
    var size = ((width < height) ? width : height) / 5;
    var spinner = $('<div />').addClass('spinner img-load').css('width', size + 'px').css('height', size + 'px');

    spinner.css('top', height / 2 - spinner.height() / 2 + 'px');
    spinner.css('left', width / 2 - spinner.width() / 2 + 'px');

    overlay.append(spinner);



    //for (var i = 1; i <= 8; i++) {
    //    var circ = $('<div/>').attr('id', 'circularG_' + i).addClass('circularG');
    //    parent.append(circ);
    //}
    //var fontSize = el.width() * 0.10; // 10% of container width

    //parent.find('i').css('font-size', fontSize);

    el.css('position', 'relative');

    el.append(overlay);
    el.find('#overlay').fadeIn();
}

function hideLoading(el) {
    el.find('#overlay').fadeOut(400, function () {
        el.find('#overlay').remove();
    });


}


/* do containsa zeby nie bral case'a pod uwage */
$.extend($.expr[':'], {
    'icontains': function (elem, i, match, array) {
        return (elem.textContent || elem.innerText || '').toLowerCase()
            .indexOf((match[3] || "").toLowerCase()) >= 0;
    }
});


/* na razie tutaj wrzucam kod do automatycznych testow */

var ajs = (function ($) {

    var $toggleButton,
        $stepListContainer,
        $clearButton,
        $playbackButton;

    var steps = [];

    var GUIElements = {
        toggleButton: "<a href='javasript:' class='test-recorder'>start</a>",
        clearButton: "<a href='javascript:' class='clear-button'>clear</a>",
        playbackButton: "<a href='javascript:' class='playback-button'>playback</a>",
        stepListContainer: "<ul class='step-list'></ul>"
    };

    function createGUI() {
        $('html').append(
        "<div class='ajs test-box'>" +
            GUIElements.toggleButton +
             GUIElements.clearButton +
             GUIElements.playbackButton +
             GUIElements.stepListContainer +
        "</div>");
    }

    function assignSelectors() {
        $toggleButton = $(".test-recorder");
        $stepListContainer = $('.step-list');
        $clearButton = $('.ajs .clear-button');
        $playbackButton = $('.ajs .playback-button');
    }

    function simulateClick(x, y) {
        jQuery(document.elementFromPoint(x, y)).click();
    }

    function bindEvents() {

        $('*').on('click', function (ev) {
            if (recording) {
                var target = $(ev.target);
                if (!target.parents(".ajs").length) {
                    var id = new Date().getTime();
                    target.attr("data-test-id", id);
                    addStepToList(ev);
                    steps.push({ id: id, event: ev });
                    $("*[data-test-id='" + id + "']").on(ev.type, function () { alert(1); });
                    ev.preventDefault();
                    ev.stopPropagation();
                }
            }
        });

        $toggleButton.on('click', function () {
            toggleRecording();
        });

        $clearButton.on('click', function () {
            clearSteps();
        });

        $playbackButton.on('click', function () {
            playback();
        });
    }

    function clearSteps() {
        $stepListContainer.html('');
    }

    function addStepToList(ev) {
        //console.log(steps);
        $stepListContainer.append("<li>" + ev.toElement + " " + ev.type + "</li>");
    }

    function toggleRecording() {
        recording = !recording;
        if (recording)
            $toggleButton.text("stop");
        else
            $toggleButton.text("start");
    }

    function playback() {
        for (var i = 0; i < steps.length; i++) {
            setTimeout(triggerEvent, 2000 * (i + 1), i);
        }
    }

    function triggerEvent(i) {
        console.log('step ' + i + ' playback');
        console.log('steps id:  ' + steps[i].id);
        console.log(steps[i].event);
        simulateClick(steps[i].event.pageX, steps[i].event.pageY)

        //$("*[data-test-id='" + steps[i].id + "']").trigger(steps[i].event.type);
    }

    function startRecording() {
        recording = true;
    }

    function stopRecording() {
        recording = false;
    }

    var recording = false;

    return {
        start: function () {
            createGUI();
            assignSelectors();
            bindEvents();
        }
    }
}(jQuery));



$(function () {
    PrepareHarmonogram();

    //ajs.start();

});



