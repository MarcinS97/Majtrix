function cntWniosekModal() {
    

}

function $el(id) {
    return $("[id$='" + id + "']");
}

function cntZmianaWariantuModal() {

}

function cntWarianty() {
    $(".tdSkladka input[type=checkbox]").on("click", function () {
        var that = $(this);
        var parentTr = that.parents("tr");
        var parentTd = that.parents("td");

        var sum = parseFloat(parentTd.data("value"));

        var checked = that.prop("checked");
        $(".tdSkladka input[type=checkbox]").prop("checked", false);
        that.prop("checked", checked);


        var $sum = parentTr.find(".sum");
        $sum.data("value", sum);
    });
    $(".tdSkladkaPlus input[type=checkbox]").on("click", function () {
        var that = $(this);
        var parentTr = that.parents("tr");
        var parentTd = that.parents("td");
        var checked = that.prop("checked");
        var skladka = parentTr.find(".tdSkladka input[type=checkbox]");
        var skladkaSum = parseFloat(skladka.parents("td").data("value"));
        var sum = parseFloat(parentTd.data("value"));
        var $sum = parentTr.find(".sum");
        var newSum = skladkaSum;// = oldSum;
        if (checked) {
            $(".tdSkladkaPlus input[type=checkbox]").prop("checked", false);//.prop("disabled", true);
            //$(".tdSkladka input[type=checkbox]").prop("checked", false);
            that.prop('checked', true);//.prop('disabled', false);
            //skladka.prop('checked', true);
            newSum += sum;
            //skladka.prop('checked', true);
        }
        //$sum.text(newSum);
    });
}

function cntWypowiedzenieModal() {
    var oczekiwaniaChecked = $(".OdrzOczekiwania input[type=checkbox]").prop('checked');
    var innaChecked = $(".OdrzInnaPrzyczyna input[type=checkbox]").prop('checked');


    $(".OdrzOczekiwaniaUwagi").toggleClass('hidden', !oczekiwaniaChecked);
    $(".OdrzInnaPrzyczynaUwagi").toggleClass('hidden', !innaChecked);

    $(".OdrzOczekiwania input[type=checkbox]").on("click", function () {
        var that = $(this);
        var checked = that.prop('checked');
        $(".OdrzOczekiwaniaUwagi").toggleClass('hidden', !checked);
    });

    $(".OdrzInnaPrzyczyna input[type=checkbox]").on("click", function () {
        var that = $(this);
        var checked = that.prop('checked');
        console.log(checked);
        $(".OdrzInnaPrzyczynaUwagi").toggleClass('hidden', !checked);
    });

    $('.DataZakonczenia .datepicker').on("change", function () {
        var that = $(this);
        //var date = that.datepicker('getDate');
        //that.datepicker('update', date);

        var dateStr = that.val();
        var date = new Date(dateStr);
        var lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        //console.log(lastDayOfMonth);

        dateStr = padStr(lastDayOfMonth.getFullYear()) + '-' +
                  padStr(1 + lastDayOfMonth.getMonth()) + '-' +
                  padStr(lastDayOfMonth.getDate());

        setTimeout(function () {
            console.log(dateStr);
        }, 0);
        that.val(dateStr);


        //that.val('2016-01-01');

    });



    function padStr(i) {
        return (i < 10) ? "0" + i : "" + i;
    }

}

function cntWnioskiUrlopoweSelect() {
    $('.wniosek-info').unbind('click');
    $(".wniosek-info").on("click", function (ev) {
        var that = $(this);
        that.parent().find(".text").slideToggle();
        ev.stopPropagation();
        ev.preventDefault();
    });

}

function ttime() {
    function pad(n) { return n < 10 ? '0' + n : n }
    var BASE = 14;
    var currentDate = new Date();
    var hours = currentDate.getHours() + BASE;
    var minutes = currentDate.getMinutes();
    var seconds = currentDate.getSeconds();
    console.log(pad(hours) + ':' + pad(minutes) + ':' + pad(seconds) + ' UTC (Universal Coordinated T-Dogg)');
}

function searchClick() {
    var $search = $('.navbar .search');
    if (!$search.is(":visible")) {
        $search.toggle(200);
        $search.focus();
    } else {
        if($search.val().length > 0){
            $(".btn-search").click();
        } else {
            $search.toggle(200);
        }
    }
}

function prepareMaster() {

    $(function () {
        $('.sidebar a.selected').parents("ul.dropdown-menu").toggle(0);
    });

    $(".sidebar ul > li > a").on('click', function () {
        if ($('.sidebar').hasClass('min')) {
            
        } else {
            $(this).parent().find("ul.dropdown-menu").toggle(300);
        }
    });
    $('.sidebar ul > li,.sidebar ul > li > ul').hover(function () {
        if ($('.sidebar').hasClass('min')) {
            $(this).toggleClass('toggle');
            $(this).find('ul.dropdown-menu').show();
        }
    }, function () {
        if ($('.sidebar').hasClass('min')) {
            $(this).toggleClass('toggle');
            $(this).find('ul.dropdown-menu').hide();
        }
    });

    //$('.appBarToggler').on('click', function (ev) {
    //    $('body').toggleClass('app-bar-visible');
    //    ev.stopPropagation();
    //});

    //$('#appBar').on('click', function (ev) {
    //    ev.stopPropagation();
    //});


    //$(window).click(function () {
    //    $('body').toggleClass('app-bar-visible', false);
    //});

    $(".card").on("click", function () {
        var that = $(this); 
        $(".top-cards .card").addClass("small");
        $(".top-cards .card").removeClass("selected");
        that.toggleClass("selected");
        $(".ub").hide(300);
        var tg = that.data('toggle');
        $(tg).show(300);

    });

    $('.sidebar-hide').on('click', function (ev) {
        toggleSidebar(false);
        ev.stopPropagation();
    });

    $('.sidebar-show').on('click', function (ev) {
        toggleSidebar(true);
        ev.stopPropagation();

    });

    $('.portal-search').keypress(function (e) {
        if (e.which == 13) {
            $(".btn-search").click();
        }
    });

    // media query event handler
    if (matchMedia) {
        var mq = window.matchMedia("(max-width: 1279px)");
        //var mq1920 = window.matchMedia("(max-width: 1920px)");
        mq.addListener(WidthChange);
        WidthChange(mq);
    }

    // media query change
    function WidthChange(mq) {
        if (mq.matches) {
            // window width is at least 500px
            toggleSidebar(false);

        } else {
            // window width is less than 500px
            toggleSidebar(true);
        }
    }

    function toggleSidebar(b) {
        $('.sidebar').toggleClass('min', !b);
        $('body').toggleClass('sidebar-min', !b);
        //if ($('.sidebar').is(":visible")) {
           
            //$('body').toggleClass('sidebar-visible', b);
        $('.sidebar-hide').toggleClass('hidden', !b);
        $('.sidebar-show').toggleClass('hidden', b);
        //}
    }


}

function sidebarEdit() {
    $('.sidebar a').parents("ul.dropdown-menu").show(300);
}

function sidebarCancel() {
    $('.sidebar a').parents("ul.dropdown-menu").hide(300);
}

function handleSidebar() {
    if ($('.sidebar').hasClass('edit')) {
        $('.sidebar a').parents("ul.dropdown-menu").show(0);
    }
    $(".sidebar ul > li > a").on('click', function () {
        $(this).parent().find("ul.dropdown-menu").toggle(300);
    });
}

function prepareKiosk() {
    console.log('kiosk');
    $('#appBar').append(" <a id='app-opener' class='row' href='javascript:'>" +
    "<span class='col-left'>" +
        "<i class='fa fa-chevron-left'></i>" +
    "</span>" +
"</a>");

    $('#app-opener').on('click', function () {
        $(this).find("i").toggleClass('fa-chevron-left').toggleClass('fa-chevron-right');
        $('.app-link').toggleClass('opened');
    });


}

function prepareWnioskiZdalna() {
    $('.cntWniosekUrlopowy .Od').on('change', function () {
        var val = $(this).find('input').val();
        $('.cntWniosekUrlopowy .Do input').val(val);
    });
}

function prepareChat(employeeId, webMethodsPathParam) {
    // dać 'var' potem
    var socialBar = {
        config: {
            selectors: {
                bar: '.social-bar',
                opener: '.social-bar-opener',
                friend: '.social-bar .friends .friend'
            },
            openedClass: 'opened',
            msgLoadInterval: 1500
        },
        init: function (config) {
            if (config && typeof (config) == 'object') {
                $.extend(schedule.config, config);
            }

            socialBar.$opener = $(socialBar.config.selectors.opener);
            socialBar.$bar = $(socialBar.config.selectors.bar);
            socialBar.$friends = $(socialBar.config.selectors.friend);
            
            socialBar.$friends.on('click', function () {
                var self = $(this);
                var name = self.data('name'),
                    id = self.data('id'),
                    avatar = self.find('.avatar');
                socialBar.createChat(id, name, avatar);
            });

            socialBar.$opener.on('click', function () {
                socialBar.$bar.toggleClass(socialBar.config.openedClass);
                if (socialBar.chats.length > 0)
                    socialBar.reposition();
            });
            
            socialBar.employeeId = employeeId;
            socialBar.webMethodsPath = webMethodsPathParam;

            //socialBar.interv = setInterval(function () {
            //    socialBar.getNewMessages();
            //}, socialBar.config.msgLoadInterval);
            socialBar.interv = null;
        },
        chats: [],
        reposition: function () {
            var x = $('.social-bar.opened').length * 260 + 4;
            for (var i = 0; i < socialBar.chats.length; i++) {
                socialBar.chats[i].setPosition(x);
                x += 310;
            }
        },
        chatExists: function(friendId){
            for (var i = 0; i < socialBar.chats.length; i++) {
                if (socialBar.chats[i].friend.id === friendId) {
                    return true;
                }
            }
        },
        createChat: function (id, name, avatar) {
            var chat = new Chat({ id: id, name: name, avatar: avatar });

            if (!socialBar.chatExists(id)) {
                chat.open();
                socialBar.chats.push(chat);
                socialBar.reposition();
            }

            //console.log(socialBar.interv);
            if (socialBar.interv == null) {
                socialBar.interv = setInterval(function () {
                    socialBar.getNewMessages();
                }, socialBar.config.msgLoadInterval);
            }
        },
        cleanup: function(chat){
            var indexToRemove = null, cleaned = false;
            while (!cleaned) {
                cleaned = true;
                for (var i = 0; i < socialBar.chats.length; i++) {
                    if (socialBar.chats[i].destroy) {
                        socialBar.chats.splice(i, 1);
                        cleaned = false;
                        break;
                    }
                }
            }
            if (socialBar.chats.length > 0) {
                socialBar.reposition();
            }
            else {
                clearInterval(socialBar.interv);
                socialBar.interv = null;
            }
        },
        getNewMessages: function () {
            //console.log('xd');
            if (socialBar.loadingMsgs)
                return false;

            socialBar.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetAllMessages";

            var requests = [];

            for (var i = 0; i < socialBar.chats.length; i++) {
                var chat = socialBar.chats[i];
                if (!chat.initialize && !chat.destroy) {
                    requests.push({
                        employeeId: socialBar.employeeId,
                        friendId: chat.friend.id,
                        lastId: chat.lastId
                    });
                }
            }
            //console.log(requests);
            
            var data = {
                requests: requests
            }
            var encoded = JSON.stringify(data);

            $.ajax({
                type: "POST",
                url: url,
                data: encoded,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    socialBar.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);

                    //console.log(msgs);
                    for (var i = 0; i < msgs.length; i++) {
                        var msg = msgs[i];
                        //console.log(msg);
                        var chat = socialBar.findChat(msg.employeeId, msg.friendId);
                        if (!chat.initialize && !chat.destroy) {

                            var t = chat.$content.scrollTop();
                            var h = chat.$content.prop("scrollHeight");

                            chat.appendMessage(msg);

                            chat.lastId = msg.id;

                            if (h - t < 500)
                                chat.scrollDown();
                        }   
                    }

                    //if (msgs.length > 0) {
                    //    callback(msgs);
                    //}
                },
                error: function (response) {
                    console.log(response);
                }
            });
        },
        findChat: function (employeeId, friendId) {
            //console.log(employeeId + ' ' + friendId);
            for (var i = 0; i < socialBar.chats.length; i++) {
                //console.log(socialBar.chats[i].friend.id);
                if (socialBar.chats[i].friend.id == friendId ||
                    socialBar.chats[i].friend.id == employeeId) {
                    //console.log('return');
                    return socialBar.chats[i];
                }
            }
        }
    }

    socialBar.init();

    function Chat(friend) {
        this.friend = friend;
        this.self = this;
        this.createContainer = function () {
            var div = $('<div>').addClass('chat-box');

            var chatCloser = $("<a href='javascript:' class='chat-closer'><i class='fa fa-close'></i></a>");
            var header = $('<div>').addClass('header').html("<a href='javascript:'>" + friend.name + "</a>")
                .append(chatCloser);

            var content = $('<div>').addClass('content').html('');

            var input = $('<textarea/>')
                .attr({ type: 'text', id: 'chat-input', class: 'input', placeholder: 'Wiadomość...' });

            this.$header = header;
            this.$chatCloser = chatCloser;
            this.$content = content;
            this.$input = input;

            div.append(header);
            div.append(content);
            div.append(input);
            return div;
        }
        this.open = function () {
            this.initialize = true;
            var self = this;
            this.lastId = 0;
            this.$container = this.createContainer();
            

            // emojiarea
            var el = this.$input.emojioneArea({
                template: "<filters/><editor/><tabs/>",
                autoHideFilters: true,
                hideSource: true,
                events: {
                    keydown: function (editor, e) {
                        if (e.keyCode === 13) {
                            var msg = editor.html().trim();
                            if (msg) {
                                self.sendMessage(msg);
                                editor.text('');
                            }
                            e.stopPropagation();
                            e.preventDefault();
                        }
                    },
                    emojibtn_click: function (button, event) {
                        //console.log('event:emojibtn.click, emoji=' + button.children().data("name"));
                        //$('.emojionearea-picker').toggleClass('hidden');

                        // tu jakby ktos chcial zamykanie po kliknieciu od razu

                    }
                }
            });

            el[0].emojioneArea.on("emojibtn.click", function (btn) {
                this.hidePicker();
            });
            //el[0].emojioneArea.setFocus();
            //this.$input.focus();
            //$('.emojionearea-editor').addClass('focused');
            $('body').append(this.$container);

            //el[0].emojioneArea.setFocus();  
            

            this.getInitialMessages(function (msgs) {
                msgs.reverse();
                self.appendMessages(msgs);
                //console.log('initial ' + self.$content.prop("scrollHeight"));
                self.$content.scrollTop(self.$content.prop("scrollHeight"));
                //self.scrollDown(100);
                self.firstId = msgs[0].id;
                self.lastId = msgs[msgs.length - 1].id;
                self.initialize = false;
            });

            this.index = socialBar.chats.length;

            this.$chatCloser.on('click', function () {
                self.close();
            })
            this.$content.on('scroll', function (e) {
                var pos = self.$content.scrollTop();
                if (pos < 50) {
                    self.getOldMessages(function (msgs) {
                        self.firstId = msgs[0].id;
                        msgs.reverse();
                        var oldHeight = self.$content.prop('scrollHeight');
                        self.prependMessages(msgs);
                        var newHeight = self.$content.prop('scrollHeight');
                        var pos = newHeight - oldHeight;
                        self.$content.scrollTop(pos);
                    });
                }
            })

            this.$header.on('click', function () {
                self.toggle();
            });

            //this.interv = setInterval(function () {
            //    self.getNewMessages(function (msgs) {
            //        self.appendMessages(msgs);
            //        self.lastId = msgs[msgs.length - 1].id;
            //        self.scrollDown();
            //    });
            //}, 1000);
        }
        this.close = function () {
            this.$container.remove();
            clearInterval(this.interv);
            this.destroy = true;
            socialBar.cleanup();
        }
        this.setPosition = function (pos) {
            this.$container.css('right', pos);
        }
        this.sendMessage = function (msg) {
            var self = this;

            var ChatMessage = {
                employeeId: socialBar.employeeId,
                friendId: this.friend.id,
                msg: msg
            };

            var json = {
                data: ChatMessage
            };

            var encodedData = JSON.stringify(json);
            var url = socialBar.webMethodsPath + "/SendMessage";

            $.ajax({
                type: "POST",
                url: url,
                data: encodedData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    self.getNewMessages(function (msgs) {
                        self.appendMessages(msgs);
                        self.lastId = msgs[msgs.length - 1].id;
                        self.scrollDown();
                    });
                },
                error: function (response) {
                    console.log(response);
                }
            });

        }
        this.getInitialMessages = function (callback) {
            var self = this;
            if (self.loadingMsgs)
                return false;

            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetInitialMessages";

            $.ajax({
                type: "POST",
                url: url,
                data: '{"employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.getNewMessages = function (callback) {            
            var self = this;
            if (self.loadingMsgs || socialBar.loadingMsgs)
                return false;

            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetMessages";
            $.ajax({
                type: "POST",
                url: url,
                data: '{"lastId" : "' + self.lastId + '", "employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.getOldMessages = function (callback) {
            var self = this;
            if (self.loadingMsgs || socialBar.loadingMsgs)
                return false;
            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetOldMessages";
            $.ajax({
                type: "POST",
                url: url,
                data: '{"firstId" : "' + self.firstId + '", "employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        //console.log(msgs);
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.appendMessages = function (msgs) {
            for (var i = 0; i < msgs.length; i++) {
                this.appendMessage(msgs[i]);
            }
        }
        this.appendMessage = function (msg) {
            var span = $('<span>').addClass('msg').html(msg.msg).attr('title', 'Wysłano: ' + msg.sentDate).attr('data-toggle', 'tooltip').attr('data-container', 'body');
            span.tooltip();
            var line = $('<div>').html(span);
            if (msg.employeeId == socialBar.employeeId) {
                line.addClass('me');
            } else {
                line.addClass('friend');
            }
            this.$content.append(line);
        }
        this.prependMessages = function (msgs) {
            for (var i = 0; i < msgs.length; i++) {
                this.prependMessage(msgs[i]);
            }
        }
        this.prependMessage = function (msg) {
            var span = $('<span>').addClass('msg').text(msg.msg).attr('title', 'Wysłano: ' + msg.sentDate).attr('data-toggle', 'tooltip').attr('data-container', 'body');
            span.tooltip();
            var line = $('<div>').html(span);
            if (msg.employeeId == socialBar.employeeId) {
                line.addClass('me');
            } else {
                line.addClass('friend');
            }
            this.$content.prepend(line);
        }
        this.scrollDown = function (d) {
            var duration = d || 300;
            var self = this;

            //console.log('scrollDown ' + self.$content.prop("scrollHeight"));
            self.$content.animate(
                {
                    scrollTop: self.$content.prop("scrollHeight")
                },
                {
                    duration: duration,
                    specialEasing: {
                        scrollTop: "swing"
                    }
                }
            );
        }
        this.toggle = function () {
            this.$content.toggle(0);
            this.$container.find('.emojionearea.input').toggle(0);
        }
    }
    //socialBar.createChat(1, 'Wujciów Tomasz');
    //socialBar.createChat(2189, 'Brudziak Wiktor');
    //socialBar.createChat(2212, 'Dobek Rafał');

}

function prepareChatSomedayImGonnaFinishItThatWay(employeeId, webMethodsPathParam) {
    
    // Observers list

    function ChatList() {
        this.chatList = [];
    }

    ChatList.prototype.add = function (obj) {
        return this.chatList.push(obj);
    }

    ChatList.prototype.count = function () {
        return this.chatList.length;
    }

    ChatList.prototype.get = function (index) {
        if (index > -1 && index < this.chatList.length) {
            return this.chatList[index];
        }
    }

    ChatList.prototype.indexOf = function (obj, startIndex) {
        var i = startIndex;

        while (i < this.chatList.length) {
            if (this.chatList[i] === obj) {
                return i;
            }
            i++;
        }

        return -1;
    }

    ChatList.prototype.removeAt = function (index) {
        this.chatList.splice(index, 1);
    }


    // Subject

    function ChatHandler() {
        this.chats = new ChatList();
    }

    ChatHandler.prototype.addChat = function (chat) {
        this.chats.add(chat);
    }

    ChatHandler.prototype.removeChat = function (chat) {
        this.chats.removeAt(this.chats.indexOf(chat, 0));
    }

    ChatHandler.prototype.notify = function (context) {
        var chatCount = this.chats.count();
        for (var i = 0; i < chatCount; i++) {
            this.chats.get(i).update(context);
        }
    }


    function Chat() {
        this.update = function () {
            alert(1);
        }
    }



    //$('.social-bar-opener').on('click', function () {
    //    $('.social-bar').toggleClass('opened');
    //});

}

function prepareFullChat(employeeId, webMethodsPathParam) {
    // dać 'var' potem
    var socialBar = {
        config: {
            selectors: {
                friend: '.cntChat .friend'
            }
        },
        init: function (config) {
            if (config && typeof (config) == 'object') {
                $.extend(schedule.config, config);
            }
            socialBar.$friends = $(socialBar.config.selectors.friend);

            socialBar.$friends.on('click', function () {
                var self = $(this);
                var name = self.data('name'),
                    id = self.data('id'),
                    avatar = self.find('.avatar');

                socialBar.$friends.toggleClass('active', false);
                self.toggleClass('active', true);

                socialBar.createChat(id, name, avatar);
            });

            socialBar.employeeId = employeeId;
            socialBar.webMethodsPath = webMethodsPathParam;
        },
        createChat: function (id, name, avatar) {
            socialBar.chat = new Chat({ id: id, name: name, avatar: avatar });
            socialBar.chat.open();
        },
        chat: null
    }

    socialBar.init();

    function Chat(friend) {
        this.friend = friend;
        this.self = this;
        this.loadContainer = function () {
            //var div = $('<div>').addClass('chat-box');
            var div = $('.cntChat .chat-container');
            //this.$header = $('.cntChat .chat-container #input');//header;
            //this.$chatCloser = chatCloser;
            this.$content = $('.cntChat .chat-container #content');//content;
            this.$input = $('.cntChat .chat-container #input textarea');

            //div.append(header);
            //div.append(content);
            //div.append(input);
            return div;
        }
        this.open = function () {
            this.initialize = true;
            var self = this;
            this.lastId = 0;
            this.$container = this.loadContainer();


            this.$content.html('');

            var el = this.$input.emojioneArea({
                template: "<filters/><editor/><tabs/>",
                autoHideFilters: true,
                hideSource: true,
                events: {
                    keydown: function (editor, e) {
                        if (e.keyCode === 13) {
                            var msg = editor.html().trim();
                            if (msg) {
                                self.sendMessage(msg);
                                editor.text('');
                            }
                            e.stopPropagation();
                            e.preventDefault();
                        }
                    },
                    emojibtn_click: function (button, event) {
                        //console.log('event:emojibtn.click, emoji=' + button.children().data("name"));
                        //$('.emojionearea-picker').toggleClass('hidden');

                        // tu jakby ktos chcial zamykanie po kliknieciu od razu

                    }
                }
            });

            el[0].emojioneArea.on("emojibtn.click", function (btn) {
                this.hidePicker();
            });

            this.getInitialMessages(function (msgs) {
                msgs.reverse();
                self.appendMessages(msgs);
                self.$content.scrollTop(self.$content.prop("scrollHeight"));
                self.firstId = msgs[0].id;
                self.lastId = msgs[msgs.length - 1].id;
                self.initialize = false;
            });


            this.$content.on('scroll', function (e) {
                var pos = self.$content.scrollTop();
                if (pos < 50) {
                    self.getOldMessages(function (msgs) {
                        self.firstId = msgs[0].id;
                        msgs.reverse();
                        var oldHeight = self.$content.prop('scrollHeight');
                        self.prependMessages(msgs);
                        var newHeight = self.$content.prop('scrollHeight');
                        var pos = newHeight - oldHeight;
                        self.$content.scrollTop(pos);
                    });
                }
            })

            this.interv = setInterval(function () {
                self.getNewMessages(function (msgs) {
                    self.appendMessages(msgs);
                    self.lastId = msgs[msgs.length - 1].id;
                    self.scrollDown();
                });
            }, 1000);
        }
       
        this.sendMessage = function (msg) {
            var self = this;

            var ChatMessage = {
                employeeId: socialBar.employeeId,
                friendId: this.friend.id,
                msg: msg
            };

            var json = {
                data: ChatMessage
            };

            var encodedData = JSON.stringify(json);
            var url = socialBar.webMethodsPath + "/SendMessage";

            $.ajax({
                type: "POST",
                url: url,
                data: encodedData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    self.getNewMessages(function (msgs) {
                        self.appendMessages(msgs);
                        self.lastId = msgs[msgs.length - 1].id;
                        self.scrollDown();
                    });
                },
                error: function (response) {
                    console.log(response);
                }
            });

        }
        this.getInitialMessages = function (callback) {
            var self = this;
            if (self.loadingMsgs)
                return false;

            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetInitialMessages";

            $.ajax({
                type: "POST",
                url: url,
                data: '{"employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.getNewMessages = function (callback) {
            var self = this;
            if (self.loadingMsgs || socialBar.loadingMsgs)
                return false;

            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetMessages";
            $.ajax({
                type: "POST",
                url: url,
                data: '{"lastId" : "' + self.lastId + '", "employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.getOldMessages = function (callback) {
            var self = this;
            if (self.loadingMsgs || socialBar.loadingMsgs)
                return false;
            self.loadingMsgs = true;
            var url = socialBar.webMethodsPath + "/GetOldMessages";
            $.ajax({
                type: "POST",
                url: url,
                data: '{"firstId" : "' + self.firstId + '", "employeeId" : "' + socialBar.employeeId + '", "friendId" : "' + self.friend.id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    self.loadingMsgs = false;
                    var messages = response.d;
                    var msgs = JSON.parse(messages);
                    if (msgs.length > 0) {
                        //console.log(msgs);
                        callback(msgs);
                    }
                },
                error: function (response) {
                    console.log('error ' + response);
                }
            });
        }
        this.appendMessages = function (msgs) {
            for (var i = 0; i < msgs.length; i++) {
                this.appendMessage(msgs[i]);
            }
        }
        this.appendMessage = function (msg) {
            var span = $('<span>').addClass('msg').html(msg.msg).attr('title', 'Wysłano: ' + msg.sentDate).attr('data-toggle', 'tooltip').attr('data-container', 'body');
            span.tooltip();
            var line = $('<div>').html(span);
            if (msg.employeeId == socialBar.employeeId) {
                line.addClass('me');
            } else {
                line.addClass('friend');
            }
            this.$content.append(line);
        }
        this.prependMessages = function (msgs) {
            for (var i = 0; i < msgs.length; i++) {
                this.prependMessage(msgs[i]);
            }
        }
        this.prependMessage = function (msg) {
            var span = $('<span>').addClass('msg').text(msg.msg).attr('title', 'Wysłano: ' + msg.sentDate).attr('data-toggle', 'tooltip').attr('data-container', 'body');
            span.tooltip();
            var line = $('<div>').html(span);
            if (msg.employeeId == socialBar.employeeId) {
                line.addClass('me');
            } else {
                line.addClass('friend');
            }
            this.$content.prepend(line);
        }
        this.scrollDown = function (d) {
            var duration = d || 300;
            var self = this;

            //console.log('scrollDown ' + self.$content.prop("scrollHeight"));
            self.$content.animate(
                {
                    scrollTop: self.$content.prop("scrollHeight")
                },
                {
                    duration: duration,
                    specialEasing: {
                        scrollTop: "swing"
                    }
                }
            );
        }
    }
    //socialBar.createChat(1, 'Wujciów Tomasz');
    //socialBar.createChat(2189, 'Brudziak Wiktor');
    //socialBar.createChat(2212, 'Dobek Rafał');

}

function prepareTinyMce(id) {

    tinymce.remove(id);
    tinymce.init(
       {
           selector: id,
           body_class: 'editor-content',
           height: 500,
           width: 'auto',
           language: 'pl',
           content_css: "../Portal/Styles/editor.css",
           plugins: [
             'advlist autolink lists link image charmap print preview anchor',
             'searchreplace visualblocks code',
             'insertdatetime media table contextmenu paste code emoticons textcolor colorpicker'
           ],
           paste_data_images: true,
           convert_urls: false,
           toolbar: 'insertfile undo redo | fontselect fontsizeselect forecolor backcolor | paste | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | imgupload',
           setup: function (editor) {

               editor.on('init', function (editor) {
                   editor.target.editorCommands.execCommand("fontName", false, "sans-serif");
                   editor.target.editorCommands.execCommand("fontSize", false, "14px");
               });
               //alert(0);
               editor.addButton('imgupload', {

                   text: 'Dodaj zdjęcie',
                   icon: false,
                   onclick: function () {
                       $('#uploader').trigger('click');
                       $("#uploader").on("change", function () {
                           var that = $(this);
                           var file = that.prop('files')[0];

                           var reader = new FileReader();

                           reader.addEventListener("load", function () {
                               var base64 = reader.result;
                               editor.insertContent("<img src='" + base64 + "' />");
                           }, false);

                           if (file) {
                               var data = reader.readAsDataURL(file);
                           }
                       });
                   }
               });
           }
       });
}

function prepareArticleEditor(id) {

    $('.article-edit-modal').on('show.bs.modal', function () {

        prepareTinyMce(id);
    });
}



$(document).ready(function () {





    /* najlepsza rzecz w systemie */

    var $globalModal = $("#___ModalMessage___");
    var globalModalVal = $globalModal.val();
    if (globalModalVal != "" && globalModalVal != undefined) {
        showModal("Layout/ShowModalMessage/?msg=" + globalModalVal);
    }

    $("[data-toggle='modal']").on("click", function () {
        if ($(".modal-backdrop fade in").is(":visible")) {
            return false;
        }

        $("<div></div>").attr('id', 'modalDiv').appendTo('body');
        $.get($(this).attr('modal'), function (result) {
            $("#modalDiv").html(result);
            $('#modalDiv #myModal').modal('show');

            $("[type='date']").datepicker({
                dateFormat: "yyyy-mm-dd",
                validateDate: true

            });

            $('.modal').on('hidden.bs.modal', function () {
                $(this).remove();
            })

            $(".btnModalConfirm").on("click", function () {
                if (showGif) {
                    $(this).hide();
                    $(".btnFakeModalConfirm").show();
                    $('#myModal').modal("hide"); /* nie jestem pewien co do tego - JUAN */
                }

            });

            $(".SelectedCheckboxesV").on("click", function () {
                $(this).attr("href", $(this).attr("href").replace("_komentarz2", $(this).parents("#myModal").eq(0).find("#komentarz2").val()));
                $(this).attr("href", $(this).attr("href").replace("_komentarz", $(this).parents("#myModal").eq(0).find("#komentarz").val()));
                $(this).attr("href", $(this).attr("href").replace("_dateInModal", $(this).parents("#myModal").eq(0).find("#DateInModal").val()));
                //$(this).attr("href", $(this).attr("href").replace("_DDLInModal", $("#DDLInModal").find("option:selected").val()));
                replaceParameter($(this), "DDLInModal", $(this).parents("#myModal").eq(0).find("#DDLInModal").find("option:selected").val(), "href");
                replaceParameter($(this), "DDLInModal2", $(this).parents("#myModal").eq(0).find("#DDLInModal2").find("option:selected").val(), "href");
                $(this).attr("href", $(this).attr("href").replace("_PARAMETER", getCheckboxes()));
            });


        });
    });



    // To Też
    $(".SelectedCheckboxesVModal").on("click", function () {
        $(this).attr("href", $(this).attr("href").replace("_komentarz2", $(".komentarz2").val()));
        $(this).attr("href", $(this).attr("href").replace("_komentarz", $(".komentarz").val()));
        $(this).attr("href", $(this).attr("href").replace("_dateInModal", $(".DateInModal").val()));
        $(this).attr("modal", $(this).attr("modal").replace("_PARAMETER", getCheckboxes()));
        replaceParameter($(this), "DDLInModal2", $(this).parents("#myModal").eq(0).find("#DDLInModal2").find("option:selected").val(), "href");
        replaceParameter($(this), "DDLInModal", $("#DDLInModal").find("option:selected").val(), "href");

        //$(this).attr("href", $(this).attr("href").replace("_DDLInModal", $(".DDLInModal").find("option:selected").val()));

    });



});
