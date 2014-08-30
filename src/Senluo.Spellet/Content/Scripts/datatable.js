(function ($) {
    $.widget("kith.datatable", {

        version: "1.0.0",

        options: {
            width: '100%',
            height: 'auto',
            cls: 'table',
            checkRow: false,
            colModel: [],
            data: [],
            paging: true,
            autoLoad: true,
            remote: true,
            url: '',
            pageSize:15
        },

        _create: function () {
            var element = this.element,
                opt = this.options,
                wrapper,
                pager,
                pageInfo,
                table;

            element.css({
                width: this.options.width,
                height: this.options.height
            });
            wrapper = this.createWrapper(element);
            table = this.createTable(wrapper);

            this.addHeader(table, this.options.colModel);
            this.addTableData(table, this.options.data);

            table.addClass(this.options.cls);
            pageInfo = this.createPageInfo(wrapper);
            pageInfo.css({
                "float": 'left'
            });
            if (opt.paging) {
                pager = this.createPager(wrapper);
                pager.css({
                    "float": "right",
                    "text-align": "right"
                });
            }
            this.currentPage = 0;
            if (opt.autoLoad) {
                if (opt.paging) {
                    this.load(1);
                } else {
                    this.load();
                }
            }
            
            
        },
        load: function (page) {
            var me = this,
                opt = this.options,
                tbody = $(me.element).find('tbody');
            
            if (!opt.remote) return;
            if (me.paging && !page) return;
            if (me.paging && page == 0) return;
            if (page == me.currentPage) return;
            
            var param = {};
            if (page) {
                param.Take = opt.pageSize;
                param.Skip = (me.currentPage - 1) * opt.pageSize;
            }
            $.ajax({
                url: opt.url,
                data: param,
                success:function(res) {
                    if (res.success) {
                        if (page) {
                            me.currentPage = page;
                        }
                        tbody.empty();
                        if (!res.items) {
                            res.items = [];
                        }

                        $.each(res.items, function(i, row) {
                            me.addRowData(tbody, row);
                        });
                        me.setPageInfo(res);
                        me.setPager(res);

                    } else {
                        $.error(res.msg);
                    }
                }
            })
        },
        setPageInfo: function (result) {
            var me = this,
                opt = me.options,
                pageInfo = $(me.element).find('div[role=info]');
            if (!opt.paging) {
                pageInfo.html('显示' + result.count + '条信息');
            } else {
                var totalPage = (result.count / opt.pageSize + 0.5).toFixed();
                
                var html = [
                    "共",
                    totalPage,
                    "页/",
                    result.count,
                    "条数据，显示",
                    (me.currentPage - 1) * opt.pageSize + 1,
                    "到",
                    (me.currentPage - 1) * opt.pageSize + result.items.length,
                    "条数据"
                ].join("");
                pageInfo.html(html);

            }
        },
        setPager:function(result) {
            var me = this,
                opt = me.options,
                pager = $(me.element).find('.pagination'),
                count = result.count,
                content,
                ul = pager.find('ul');
            if (!opt.paging) return;
            var totalPage = (count / opt.pageSize + 0.5).toFixed();
            ul.empty();
            var prev = me.currentPage - 1;
            if (prev == 0) {
                prev = 1;
            }
            var next = me.currentPage + 1;
            if (next >= totalPage) {
                next = totalPage;
            }
            
            ul.append('<li class="first"><a data-target-page="'+ 1 + '"><i class="icon-first-2"></i></a></li>' +
                '<li class="prev"><a data-target-page="'+prev + '"> <i class="icon-previous"></i></a></li>');
            if (totalPage < 16) {
                for (var index = 1; index <= totalPage; index++) {
                    if (index == me.currentPage) {
                        content = "<li class='active'><a data-target-page='"+ index + "'>" + index + "</a></li>";
                    } else {
                        content = "<li><a data-target-page='" + index + "'>" + index + "</a></li>";
                    }
                    ul.append(content);
                }
            }
            ul.append('<li class="next"><a data-target-page="' + next + '"><i class="icon-next"></i></a></li>' +
                '<li class="last"><a data-target-page="' + totalPage + '"><i class="icon-last-2"></i></a></li>');
            ul.find('a').on('click', function(e) {
                var target = this;
                var targetpage = $(target).attr('data-target-page');
                try {
                    var page = parseInt(targetpage);
                    if (page) {
                        me.load(page);
                    }
                    
                } catch(e) {
                    
                }
            });
        },
        addHeader: function (container, data) {
            var thead = $("<thead/>").appendTo(container);
            var th, tr = $("<tr/>").appendTo(thead);
            $.each(data, function (index, column) {
                th = $("<th/>").addClass(column.hcls).html(column.caption).appendTo(tr);
            });
        },
        createPageInfo: function (container) {
            return $('<div role="info"></div>').appendTo(container);
        },
        createPager: function (container) {
            return $('<div class="pagination"><ul></ul></div>').appendTo(container);
        },
        createWrapper: function(container) {
            return $('<div/>').appendTo(container);
        },
        createTable: function (container) {
            return $("<table/>").appendTo(container);
        },

        addTableData: function (container, data) {
            var that = this,
                tbody = $("<tbody/>").appendTo(container);

            $.each(data, function (i, row) {
                that.addRowData(tbody, row);
            });
        },
        reload:function() {
            var me = this;
            me.load(me.currentPage);
        },
        addRowData: function (container, row) {
            var td, tr = $("<tr/>").appendTo(container);
            var me = this;
            tr.data('record', row);
            if (row.__row_class != undefined) {
                tr.addClass(row.__row_class);
            }
            $.each(this.options.colModel, function (index, val) {
                var content;
                td = $("<td/>").css("width", val.width).addClass(val.cls).appendTo(tr);
                if (val.renderer) {
                    val.renderer(me, row, td);
                } else {
                    content = row[val.field];
                    td.html(content);
                }
                


            });
        },

        _destroy: function () {
        },

        _setOption: function (key, value) {
            this._super('_setOption', key, value);
        }
    })
})(jQuery);

