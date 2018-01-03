window.utils = {  
    rootUrl: function () {
        var rooturl = '';
        if (location.host.toString().indexOf('localhost') >= 0) { rooturl = 'http://localhost:58007/'; }
        if (location.host.toString().indexOf('sandbox') >= 0) { rooturl = 'http://sandbox.vtcpay.vn/pay2.0/portalgateway/'; }

        return rooturl;
    },
    portalUrl: function () {
        return 'http://portal.alpha.pay365.vn/api/Portal/'; 
    },
    mediaUrl: function () {
        var mediaUrl = '';
        if (location.host.toString().indexOf('sandbox') >= 0) { mediaUrl = 'http://sandbox1.vtcebank.vn/Paygate3/Media/'; }
        if (location.host.toString().indexOf('paygate') >= 0) { mediaUrl = 'http://beta.365.vtc.vn/'; }
        mediaUrl = 'http://sandbox1.vtcebank.vn/Paygate3/Media/';
        return mediaUrl;
    },

    /* Asynchronously load templates located in separate .html files*/
    loadTemplate: function (views, callback) {
        var deferreds = [];
        $.each(views, function (index, view) {
            if (window[view]) {
                deferreds.push($.get('template/' + view + '.html', function (data) {
                    window[view].prototype.template = _.template(data);
                }));
            } else {
                console.log("not found");
            }
        });

        $.when.apply(null, deferreds).done(callback);
    },

    loadSubTemplate: function (views, url, callback) {
        var deferreds = [];
        $.each(views, function (index, view) {
            if (window[view]) {
                deferreds.push($.get(url + view + '.html', function (data) {
                    window[view].prototype.template = _.template(data);
                }));
            } else {
                console.log("not found");
            }
        });

        $.when.apply(null, deferreds).done(callback);
    },

    uploadFile: function (file, callbackSuccess) {

        /*var self = this;
        var data = new FormData();
        data.append('file', file);
        $.ajax({
            url: 'api/upload.php',
            type: 'POST',
            data: data,
            processData: false,
            cache: false,
            contentType: false
        })
        .done(function () {
            console.log(file.name + " uploaded successfully");
            callbackSuccess();
        })
        .fail(function () {
            self.showAlert('Error!', 'An error occurred while uploading ' + file.name, 'alert-error');
        });*/
    },

    translateLang: function (namespace) {
        //test i18n
        i18n.setDefaultNamespace(namespace); //set file resource để sử dụng cho view tương ứng
        $(".translang").i18n(); //dịch
    },
    displayValidationErrors: function (messages) {
        for (var key in messages) {
            if (messages.hasOwnProperty(key)) {
                this.addValidationError(key, messages[key]);
            }
        }
        this.showAlert('Warning!', 'Fix validation errors and try again', 'alert-warning');
    },

    addValidationError: function (field, message) {
        var controlGroup = $('#' + field).parent().parent();
        controlGroup.addClass('error');
        $('.help-inline', controlGroup).html(message);
    },

    removeValidationError: function (field) {
        var controlGroup = $('#' + field).parent().parent();
        controlGroup.removeClass('error');
        $('.help-inline', controlGroup).html('');
    },

    convertUTFStr: function (str) {
        str = str.toLowerCase();
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");
        str = str.replace(/-+-/g, "-");
        str = str.replace(/^\-+|\-+$/g, "");
        return str;
    },

    showTooltip: function (clientid, text) {
        if ($('.tooltip').length <= 0) {
            $('#tooltip').html('<div class="tooltip"><div class="tool_cont">' + text + '</div></div>');
            //$('#tooltip').html('<div class="tooltip"><div class="quote"><img src="' + this.rootUrl() + 'content/images/tooltip_quote.png"></div>' + text + '</div>');
            var top = $('#' + clientid).offset().top;
            var left = $('#' + clientid).offset().left;
            $('#tooltip').css('top', (top + 20) + 'px');
            $('#tooltip').css('left', (left + 98) + 'px');
            $('#tooltip').css('position', 'absolute');
            $('#tooltip').css('z-index', '11');
            $('#tooltip').css('display', 'block');
        }
    },

    hideTooltip: function () {
        $('.tooltip').remove();
    },

    documentHeight: function () {
        return $(document).height();
    },
    documentWidth: function () {
        return $(document).width();
    },
    windowHeight: function () {
        return $(window).height();
    },
    windowWidth: function () {
        return $(window).width();
    },

    loading: function () {
        this.unLoading();
        var html = '<div id="LoadingContainer"><div  id="Loading" style="display: none; text-align: center; overflow-y: none; vertical-align: middle;"><img src="' + this.rootUrl() + 'images/loading.gif" alt="Loading" /></div>';
        html += '<div  id="LoadingOverlay"></div>';
        html += '<style> #Loading{	width: 300px;	height: 300px;	z-index: 1400;	position: fixed;	padding: 5px;}#LoadingOverlay{	-moz-opacity: 0.8;	opacity: .80;	filter: alpha(opacity=80);	position: absolute;	z-index: 1200;	top: 0;	left: 0;	width: 100%;	height: 100%;	display: none;	background-color: #ccc;}</style></div>';
        $('body').append(html);
        $('#Loading');
        $('#LoadingOverlay').show();
        var leftOffset = (this.windowWidth() - 300) / 2;
        var topOffset = (this.windowHeight() - 300) / 2;
        $('#Loading').css('width', 300);
        $('#Loading').css('height', 300);
        $('#Loading').css('left', leftOffset);
        $('#Loading').css('top', '47%');
        $('#Loading').show();
        $('#LoadingOverlay').css('height', this.documentHeight());
    },
    unLoading: function () {
        $('#LoadingContainer').remove();
    },

    // Hàm lấy xâu định dạng theo kiểu tiền tệ: 1234123 --> 1.234.123
    formatMoney: function (argValue) {
        var comma = (1 / 2 + '').charAt(1);
        var digit = ',';
        if (comma == '.') {
            digit = '.';
        }

        var sSign = "";
        if (argValue < 0) {
            sSign = "-";
            argValue = -argValue;
        }

        var sTemp = "" + argValue;
        var index = sTemp.indexOf(comma);
        var digitExt = "";
        if (index != -1) {
            digitExt = sTemp.substring(index + 1);
            sTemp = sTemp.substring(0, index);
        }

        var sReturn = "";
        while (sTemp.length > 3) {
            sReturn = digit + sTemp.substring(sTemp.length - 3) + sReturn;
            sTemp = sTemp.substring(0, sTemp.length - 3);
        }
        sReturn = sSign + sTemp + sReturn;
        if (digitExt.length > 0) {
            sReturn += comma + digitExt;
        }
        return sReturn;
    },
    // Hàm convert chuỗi json Datetime sang Date
    // value: chuỗi jSon datetime
    jSonToDate: function (value) { value = value.replace('/Date(', ''); value = value.replace(')/', ''); var expDate = new Date(parseInt(value)); return expDate; },

    // Hàm convert chuỗi json Datetime sang chuối ngày tháng
    // value: chuỗi jSon datetime
    // option:
    //      0: dd/MM/yyyy hh:mm:ss
    //      1: dd/MM/yyyy
    //      2: hh:mm:ss dd/MM/yyyy
    //      3: yyyy/MM/dd hh:mm:ss
    //      5: hhmm
    jSonDateToString: function (value, option) {
        if (typeof (option) == 'undefined') {
            option = 0;
        }
        var expDate = this.jSonToDate(value);
        var day = expDate.getDate();
        var month = expDate.getMonth() + 1;
        var year = expDate.getFullYear();
        var hour = expDate.getHours();
        var minute = expDate.getMinutes();
        var second = expDate.getSeconds();
        if (day < 10) day = "0" + day;
        if (month < 10) month = "0" + month;
        if (hour < 10) hour = "0" + hour;
        if (minute < 10) minute = "0" + minute;
        if (second < 10) second = "0" + second;
        switch (option) {
            case 0:
                return day + '/' + month + '/' + year + ' ' + hour + ':' + minute + ':' + second;
                break;
            case 1:
                return day + '/' + month + '/' + year;
                break;
            case 2:
                return hour + ':' + minute + ':' + second + ' ' + day + '/' + month + '/' + year;
                break;
            case 3:
                return year + '/' + month + '/' + day + ' ' + hour + ':' + minute + ':' + second;
                break;
            case 4:
                return year + '/' + month + '/' + day;
                break;
            case 5:
                return day + 'h' + minute;
                break;
            default:
                return expDate.toString();
                break;
        }
    },

    //Kéo thanh crollbar lên đầu
    scrollTop: function () { $("html:not(:animated),body:not(:animated)").animate({ scrollTop: 0 }, 'slow'); },
    scrollBottom: function () { $("html:not(:animated),body:not(:animated)").animate({ scrollTop: utils.documentHeight() }, 'slow'); },
    validateDate: function (dtValue) {
        try {
            var dtRegex = new RegExp(/\b\d{1,2}[\/-]\d{1,2}[\/-]\d{4}\b/);
            var status = dtRegex.test(dtValue);
            if (!status) return status;
            var arr = dtValue.toString().split('/');
            if (arr.length != 3) return false;
            var day = parseInt(arr[0]);
            var month = parseInt(arr[1]);
            var year = parseInt(arr[2]);
            if (day < 0 || day > 31) return false;
            if (month > 12) return false;
            return true;
        } catch (e) {
            return false;
        }
    },
    // Check format email xem có chính xác hay không
    validateEmail: function (email) { var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/; return filter.test(email); },
    //Check chuỗi ký tự gồm ký tự chuẩn và số ._ 
    validateOnlyLetter: function (text) { var filter = /^[a-zA-Z]+$/; return filter.test(text); },
    //Check chuỗi ký tự gồm ký tự chuẩn và số ._ 
    validateLetter: function (text) { var filter = /^[a-zA-Z0-9]+$/; return filter.test(text); },
    //Check Password
    validateLetterPassword: function (text) { var filter = /^[a-zA-Z0-9\.\_~!@#$%^&*(:)-+=]+$/; return filter.test(text); },
    validateNumberOnly: function (text) { var filter = /^[0-9]+$/; return filter.test(text); },
    CheckOnlyNumber: function (obj, e) {
        var whichCode = (window.Event && e.which) ? e.which : e.keyCode; /*if (whichCode == 13) { this.onPlaceOrder(); return false; }*/
        if (whichCode == 9) return true;
        if ((whichCode >= 48 && whichCode <= 57) || whichCode == 8) {
            var n = obj.value.replace(/,/g, "");
            if (whichCode == 8) {
                if (n.length != 0)
                    n = n.substr(0, n.length - 1);
            }
            if (parseFloat(n) == 0) {
                n = '';
            }
            return true;
        }
        e.returnValue = false;
        return false;
    },

    //Bắt validate ngay khi keypress, cấm nhập số
    checkOnlyAlphabets: function (evt) {
        var e = window.event || evt;
        var charCode = e.which || e.keyCode;
        if (charCode > 47 && charCode < 58) {
            if (window.event) //IE
                window.event.returnValue = false;
            else //Firefox
                e.preventDefault();
        }
        return true;
    },
    // type=0:letter, 1 : number, 2:only letter, 3: password, 4 UserName
    inputExtender: function (id, type) {
        try {
            var val = $('#' + id).val();
            if (val == '' || val == 'undefined') {
                return;
            }

            var str = '';
            switch (type) {
                case 0:
                    for (var index = 0; index < val.length; index++) {
                        if (utils.validateLetter(val.charAt(index))) {
                            str += val.charAt(index);
                        }
                        $('#' + id).val(str);
                    }

                    break;
                case 1:
                    for (var index = 0; index < val.length; index++) {
                        if (utils.validateNumberOnly(val.charAt(index))) {

                            str += val.charAt(index);
                        }
                    }

                    $('#' + id).val(str);

                    break;
                case 2:
                    for (var index = 0; index < val.length; index++) {
                        if (utils.validateOnlyLetter(val.charAt(index))) {
                            str += val.charAt(index);
                        }
                        $('#' + id).val(str);
                    }

                    break;
                case 3:
                    for (var index = 0; index < val.length; index++) {
                        if (utils.validateLetterPassword(val.charAt(index))) {
                            str += val.charAt(index);
                        }
                        $('#' + id).val(str);
                    }
                    break;
                case 4:
                    for (var index = 0; index < val.length; index++) {
                        if (!utils.validateNumberOnly(val.charAt(index))) {
                            $('#' + id).val(val.replace(val.charAt(index), ''));
                        }
                    }
                    break;
            }
        }
        catch (err) { }
    },

    formDateTime: function (date) {
        var d = new Date(date);
        var currDate = d.getDate();
        var currMonth = d.getMonth();
        var currYear = d.getFullYear();
        return currDate + "-" + currMonth
            + "-" + currYear;
    },

    showPopup: function (text) {
        utils.closePopup();
        var mesHtml = utils.replateHtmlEntities(text);
        var url = this.rootUrl() + "common/PopupViewCommon?content=" + encodeURIComponent(mesHtml);
        $('#divPopup').load(url);
    },

    showPopupConfirmCodeBank: function (orderId, sign) {
        utils.closePopup();
        
        var url = this.rootUrl() + "PayPortal/pup_ConfirmCodeBank";
        $('#divPopup').load(url, { orderId: orderId, sign: sign }, function () {
            utils.unLoading();
        });
    },
    showPopupConfirmWalletLink: function (dataConfirm) {
        utils.closePopup();

        var url = this.rootUrl() + "PayPortal/pup_ConfirmWalletLink";
        $('#divPopup').load(url, { confirmData: dataConfirm }, function () {
            utils.unLoading();
        });
    },
    showInputMoneySupport: function (extendData) {
        utils.closePopup();
        var url = this.rootUrl() + "Common/PopupInputMoneySupport";
        $('#divPopup').load(url, { orderData: extendData }, function () {
            utils.unLoading();
        });
    },
    // type 1: Success, -1 Fail
    showPopupConfirm: function (text, btntext, type, excuseFunction) {
        utils.closePopup();
        //var mesHtml = utils.replateHtmlEntities(text);
        var url = this.rootUrl() + "common/PopupView?content=" + encodeURIComponent(text) + "&btnText=" + encodeURIComponent(btntext) + "&type=" + encodeURIComponent(type) + "&executeFunction=" + encodeURIComponent(excuseFunction);
        $('#divPopup').load(url);
    },
    
    //Đóng popup
    closePopup: function () {
        $('#divPopup').empty();
    },

    showbootboxdialog:function(title, content, lbok, lbcancel, link){
        bootbox.dialog({
            message: content,
            title: title,
            onEscape:true,
            buttons: {
                success: {
                    label: lbok,
                    className: "btn-success",
                    callback: function () {
                        debugger;
                        if (link != '' && link != null)
                            return window.open(link, '_blank');
                    }
                },
                danger: {
                    label: lbcancel,
                    className: "btn-default",
                    callback: function () {
                        dialog.modal('hide');
                    }
                }
            }
        });
    },

    showbootboxMessage: function (title, content, lbok) {
        bootbox.dialog({
            message: content,
            title: title,
            onEscape: true,
            buttons: {
                success: {
                    label: lbok,
                    className: "btn-default",
                    callback: function () {
                        bootbox.hideAll();
                    }
                }
            }
        });
    },
    showChangeNamePopup: function (name, accountTypeID) {
        $('BODY').append(new ChangeNamePopupView({ description: name, accountTypeID: accountTypeID }).render().el);
        var width = $('.popup_to').width();
        var height = $('.popup_to').height();
        var topOffset = (((utils.windowHeight() - height) / 2) * 100) / utils.windowHeight();
        var leftOffset = (((utils.windowWidth() - width) / 2) * 100) / utils.windowWidth();
        $('#popupwrap').css('width', width + 'px');
        $('#popupwrap').css('left', leftOffset + "%");
        $('#popupwrap').css("top", topOffset + "%");
        $('#popupwrap').css('z-index', 9999);
        $('#popupwrap').css('position', 'fixed');
        //$('#mainform').css('opacity', '0.1');
        $('#overlayPopup').css('height', utils.documentHeight());
    },

    showEmbedCodePopup: function (content, embedCodeType) {
        $('BODY').append(new EmbedCodePopupView({ content: content, embedCodeType: embedCodeType }).render().el);
        var width = $('.popup_nho').width();
        var height = $('.popup_nho').height();
        var topOffset = (((utils.windowHeight() - height) / 2) * 100) / utils.windowHeight();
        var leftOffset = (((utils.windowWidth() - width) / 2) * 100) / utils.windowWidth();
        $('#popupwrap').css('width', width + 'px');
        $('#popupwrap').css('left', leftOffset + "%");
        $('#popupwrap').css("top", topOffset + "%");
        $('#popupwrap').css('z-index', 9999);
        $('#popupwrap').css('position', 'fixed');
        $('#overlayPopup').css('height', utils.documentHeight());
    },

    showPopupDeleteProduct: function (data) {
        $('#divPopup').html(new IntegratedProductManageDeletePopupView({ data: data }).render().el);
        var width = $('.popup_mini').width();
        var height = $('.popup_mini').height();
        var topOffset = (((utils.windowHeight() - height) / 2) * 100) / utils.windowHeight();
        var leftOffset = (((utils.windowWidth() - width) / 2) * 100) / utils.windowWidth();
        $('#popupwrap').css('width', width + 'px');
        $('#popupwrap').css('left', leftOffset + "%");
        $('#popupwrap').css("top", topOffset + "%");
        $('#popupwrap').css('z-index', 1300);
        $('#popupwrap').css('position', 'fixed');
        $('#overlayPopup').css('height', utils.documentHeight());
    },

    errorMessage: function (fieldid, text) {
        this.clearErrorMessage(fieldid);
        var html = '<img class="fl" src="' + utils.rootUrl() + 'Content/images/ic4_error.gif" width="27" height="27"><div class="text_red">' + text + '</div>';
        $('#' + fieldid).html(html);
    },

    clearErrorMessage: function (fieldid) {
        $('#' + fieldid).empty();
    },

    FormatNumber: function (pSStringNumber) {
        pSStringNumber += '';
        var x = pSStringNumber.split(',');
        var x1 = x[0];
        var x2 = x.length > 1 ? ',' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + '.' + '$2');

        return x1 + x2;
    },

    setMenuSubTitle: function (text) {
        if ($('#subMenuTitle').length >= 0)
            $('#subMenuTitle').html(text);
    },

    showDetail: function (name) {
        var current = 'divRechargePaygate';
        $('.about').hide();
        if (current != name && current != '') {
            $('#' + current).hide();
        }
        if (
            !$('#' + name).is(':visible')) {
            $('#' + name).show();
            current = name;
        } else {
            $('#' + name).hide();
            current = '';
        }
    },

    hideAll: function () { $('.about').hide(); },

    refreshCaptcha: function (imgId, tokenId) {
        var captchaModel = new CaptchaModel();
        captchaModel.urlRoot += 'Get';
        captchaModel.fetch({
            success: function () {
                //Bind catpcha
                var url = captchaModel.get('url');
                var token = captchaModel.get('token');
                $('#' + imgId).attr('src', utils.rootUrl() + url);
                $('#' + tokenId).val(token);
                //End bind catpcha
            },
            error: function () {
                utils.showPopupCloseRedrirect('Hệ thống đang bận, vui lòng quay lại sau', '#');
            }
        });
    },

    //Hàm chuyển số thành chữ
    DocSo3ChuSo: function (baso) {
        var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
        var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
        var tram;
        var chuc;
        var donvi;
        var KetQua = "";
        tram = parseInt(baso / 100);
        chuc = parseInt((baso % 100) / 10);
        donvi = baso % 10;
        if (tram == 0 && chuc == 0 && donvi == 0) return "";
        if (tram != 0) {
            KetQua += ChuSo[tram] + " trăm ";
            if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
        }
        if ((chuc != 0) && (chuc != 1)) {
            KetQua += ChuSo[chuc] + " mươi";
            if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
        }
        if (chuc == 1) KetQua += " mười ";
        switch (donvi) {
            case 1:
                if ((chuc != 0) && (chuc != 1)) {
                    KetQua += " mốt ";
                }
                else {
                    KetQua += ChuSo[donvi];
                }
                break;
            case 5:
                if (chuc == 0) {
                    KetQua += ChuSo[donvi];
                }
                else {
                    KetQua += " lăm ";
                }
                break;
            default:
                if (donvi != 0) {
                    KetQua += ChuSo[donvi];
                }
                break;
        }
        return KetQua;
    },

    DocTienBangChu: function (SoTien) {
        var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
        var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
        var lan = 0;
        var i = 0;
        var so = 0;
        var KetQua = "";
        var tmp = "";
        var ViTri = new Array();
        if (SoTien < 0) return "Số tiền âm !";
        if (SoTien == 0) return "Không đồng !";
        if (SoTien > 0) {
            so = SoTien;
        }
        else {
            so = -SoTien;
        }
        if (SoTien > 8999999999999999) {
            //SoTien = 0;
            return "Số quá lớn!";
        }
        ViTri[5] = Math.floor(so / 1000000000000000);
        if (isNaN(ViTri[5]))
            ViTri[5] = "0";
        so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
        ViTri[4] = Math.floor(so / 1000000000000);
        if (isNaN(ViTri[4]))
            ViTri[4] = "0";
        so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
        ViTri[3] = Math.floor(so / 1000000000);
        if (isNaN(ViTri[3]))
            ViTri[3] = "0";
        so = so - parseFloat(ViTri[3].toString()) * 1000000000;
        ViTri[2] = parseInt(so / 1000000);
        if (isNaN(ViTri[2]))
            ViTri[2] = "0";
        ViTri[1] = parseInt((so % 1000000) / 1000);
        if (isNaN(ViTri[1]))
            ViTri[1] = "0";
        ViTri[0] = parseInt(so % 1000);
        if (isNaN(ViTri[0]))
            ViTri[0] = "0";
        if (ViTri[5] > 0) {
            lan = 5;
        }
        else if (ViTri[4] > 0) {
            lan = 4;
        }
        else if (ViTri[3] > 0) {
            lan = 3;
        }
        else if (ViTri[2] > 0) {
            lan = 2;
        }
        else if (ViTri[1] > 0) {
            lan = 1;
        }
        else {
            lan = 0;
        }
        for (i = lan; i >= 0; i--) {
            tmp = this.DocSo3ChuSo(ViTri[i]);
            KetQua += tmp;
            if (ViTri[i] > 0) KetQua += Tien[i];
            if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
        }
        if (KetQua.substring(KetQua.length - 1) == ',') {
            KetQua = KetQua.substring(0, KetQua.length - 1);
        }
        KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
        KetQua += " đồng";
        return KetQua;//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
    },

    // lấy ngôn ngữ hiện tại
    getCurrentLanguage: function () {
        var param = "l";
        param = param.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + param + "=([^&#]*)"),
            results = regex.exec(location.search);
        var lang = (results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " ")));

        if (lang == null || lang == "") {
            lang = 'vi';
        }

        return lang;
    },

    RedirectUriByLang: function (lang) {
        debugger
        // lay lang hien tai
        var currlang = utils.getCurrentLanguage();

        // meu lang =currlang thi giu nguyen link
        if (lang == currlang) {
            window.location = window.location.href;
            return;
        }

        // doi lang thi lay url moi
        var url = '';
        var url_current = window.location.href;
        switch (lang) {
            case 'en':
                if (url_current.indexOf('?') >= 0)
                    url = window.location.href + '&l=en';
                else
                    url = window.location.href + '?l=en';
                if (window.location.href == window.appPath) {
                    if (url_current.indexOf('?') >= 0)
                        url = window.location.href.substring(0, window.location.href.length - 1) + '&l=en';
                    else
                        url = window.location.href.substring(0, window.location.href.length - 1) + '?l=en';
                }
                break;
            default:
                if (url_current.indexOf('?') >= 0)
                    url = window.location.href + '&l=vn';
                else
                    url = window.location.href + '?l=vn';
                if (window.location.href == window.appPath) {
                    if (url_current.indexOf('?') >= 0)
                        url = window.location.href.substring(0, window.location.href.length - 1) + '&l=vn';
                    else
                        url = window.location.href.substring(0, window.location.href.length - 1) + '?l=vn';
                }
        }
        window.location = url;
    },
    //Mở popup gọi từ CodeBehide
    showPopupCS: function (text) {
        var html = '<div id="popupwrap" class="popup_mini"><div class="c2_naptien_title"><p style="cursor: default;">THÔNG BÁO</p><a href="javascript:void(0);" onclick="utils.hidePopupCS()" id="divClose" class="c2_naptien_link">Đóng</a><div class="clear"></div></div>' +
            '<div style="padding: 10px 10px 20px 10px; line-height: 20px; text-align: center;">' + text + '</div></div>' +
            '<div id="overlayPopup"></div>' +
            '<style id="styleoverlayPopup" type="text/css">#overlayPopup{position: absolute;z-index: 1200;top: 0;left: 0;width: 100%;display: block;opacity: .80;background: #ccc;filter: alpha(opacity=60);-moz-opacity: 0.8;}</style>';

        $('#divPopup').html(html);
        var width = $('.popup_mini').width();
        var height = $('.popup_mini').height();
        var topOffset = (((utils.windowHeight() - height) / 2) * 100) / utils.windowHeight();
        var leftOffset = (((utils.windowWidth() - width) / 2) * 100) / utils.windowWidth();
        $('#popupwrap').css('width', width + 'px');
        $('#popupwrap').css('left', leftOffset + "%");
        $('#popupwrap').css("top", topOffset + "%");
        $('#popupwrap').css('z-index', 1300);
        $('#popupwrap').css('position', 'fixed');
        $('#overlayPopup').css('height', utils.documentHeight());
    },
    
    //Đóng popup gọi từ CodeBehide
    hidePopupCS: function () {
        $('#divPopup').empty();
    },
    

    replaceAll: function (sources, strTarget, strSubString) {
        var strText = sources;
        var intIndexOfMatch = strText.indexOf(strTarget);

        // Keep looping while an instance of the target string
        // still exists in the string.
        while (intIndexOfMatch != -1) {
            // Relace out the current instance.
            strText = strText.replace(strTarget, strSubString)

            // Get the index of any next matching substring.
            intIndexOfMatch = strText.indexOf(strTarget);
        }

        return (strText);
    },

    formatString: function (str, param) {

        var args = param.toString().split(',');
        for (var i = 0; i < args.length; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "");
            str = utils.replaceAll(str, '{' + i + '}', args[i].toString());
        }
        return str;
    },

    // Banner
    loadInner_BannerDoubleClick: function () {
        var href = window.location.href;
        var tienich = href.indexOf("tien-ich");
        var tintuc = href.indexOf("tin-tuc");
        var vidientu = href.indexOf("vi-dien-tu");

        var html = "";
        if (tintuc > 0) {            
            utils.bannerDoubleClick_News();
        }

        if (tienich > 0) {
            utils.bannerDoubleClick_Utility();
        }

        if (vidientu > 0) {
            utils.bannerDoubleClick_BottomRight();
        }

    },
    bannerDoubleClick_News: function () {
        var m;
        ShowDoubleClickNew = function (no) {
            var numberCurrent = 0;            
            clearTimeout(m);
            console.log("ShowDoubleClickNew");

            $("#pageNews a").removeClass('active');
            var total = $(".allslide_new").length;
            $(".allslide_new").hide();
            $("#p" + no).show();
            var total = $(".allslide_new").length;
            $("#page" + no).addClass('active');
            var pageNew = no < total ? parseInt(no, 10) + 1 : 1;
            m = setTimeout('ShowDoubleClickNew(' + pageNew + ')', 6 * 1000);
        }

        var html = "";        
        // VTCPAY_Tintuc_Topright_01_300x300
        html += "<div id='p1' class='allslide_new'><div id='div-gpt-ad-1432114310346-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432114310346-0'); });";
        html += "</script>";
        html += "</div></div>";
        // VTCPAY_Tintuc_topright_02_300x300
        html += "<div id='p2' class='allslide_new' style='display:none'><div id='div-gpt-ad-1432115586166-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432115586166-0');});";
        html += "</script>";
        html += "</div></div>";
        // VTCPAY_Tintuc_Topright_03_300x300
        html += "<div id='p3' class='allslide_new' style='display:none'><div id='div-gpt-ad-1432115689906-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432115689906-0');});";
        html += "</script>";
        html += "</div></div>";

        // pager
        html += '<div id="pageNews" style="text-align: center; margin:56px 0px 0px 40px;" class="cycle-pager">';
        html += '<span><a onclick="ShowDoubleClickNew(1)" id="page1" class="active"></a></span>';
        html += '<span><a onclick="ShowDoubleClickNew(2)" id="page2"></a></span>';
        html += '<span><a onclick="ShowDoubleClickNew(3)" id="page3"></a></span>';
        html += '</div>';

        $("#advertising").html(html);

        ShowDoubleClickNew("1");

        //$('#pageNews span a').live('click', function (e) {
        //    clearTimeout(m);
        //    console.log("ShowDoubleClickNew");
        //    var no = $(e.target).attr("id").split('page')[1];            
        //    ShowDoubleClickNew(parseInt(no));
        //});
    },
    bannerDoubleClick_Utility: function () {
        var t;
        ShowDoubleClick = function (no) {            
            var numberCurrent = 0;
            clearTimeout(t);
            console.log("ShowDoubleClick");

            $("#pageUltity a").removeClass('active');
            var total = $(".allslide_utility").length;
            $(".allslide_utility").hide();
            $("#t" + no).show();
            var total = $(".allslide_utility").length;
            $("#page_ulity" + no).addClass('active');
            var pageNew = no < total ? parseInt(no, 10) + 1 : 1;
            t = setTimeout('ShowDoubleClick(' + pageNew + ')', 6 * 1000);            
        }

        var html = "";                
        // VTCPAY_Tienich_bottomright_01_270x250
        html += "<div id='t1' class='allslide_utility'><div id='div-gpt-ad-1432115080670-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432115080670-0');});";
        html += "</script>";
        html += "</div></div>";
        // VTCPAY_Tienich_bottomright_02_270x250
        html += "<div id='t2' class='allslide_utility' style='display:none'><div id='div-gpt-ad-1432115934308-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432115934308-0');});";
        html += "</script>";
        html += "</div></div>";
        // VTCPAY_Tienich_Bottomright_03_270x250
        html += "<div id='t3' class='allslide_utility' style='display:none'><div id='div-gpt-ad-1432116040337-0' style='height: 250px; width: 270px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432116040337-0');});";
        html += "</script>";
        html += "</div></div>";

        // pager
        html += '<div id="pageUltity" style="text-align: center; margin: 5px 0px 0px 0px;" class="cycle-pager">';
        html += '<span><a onclick="ShowDoubleClick(1)" id="page_ulity1" class="active"></a></span>';
        html += '<span><a onclick="ShowDoubleClick(2)" id="page_ulity2"></a></span>';
        html += '<span><a onclick="ShowDoubleClick(3)" id="page_ulity3"></a></span>';
        html += '</div>';

        $("#advertising").html(html);
        
        ShowDoubleClick("1");        
    },
    bannerDoubleClick_BottomRight: function () {
        $(".allslide_bottom").remove();
        var html = "";
        // VTCPAY_Vidientu_bottomright_01_680x100
        html += "<div id='b1' class='allslide_bottom'><div id='div-gpt-ad-1432114852697-0' style='height: 100px; width: 680px;'>";
        html += "<script type='text/javascript'>";
        html += "googletag.cmd.push(function () { googletag.display('div-gpt-ad-1432114852697-0');});";
        html += "</script>";
        html += "</div></div>";        

        $("#content .page").append(html);       
    },
    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    ReplaceAll: function (sources, strTarget, strSubString) {
        var strText = sources;

        var intIndexOfMatch = strText.indexOf(strTarget);

        // Keep looping while an instance of the target string
        // still exists in the string.
        while (intIndexOfMatch != -1) {
            // Relace out the current instance.
            strText = strText.replace(strTarget, strSubString)

            // Get the index of any next matching substring.
            intIndexOfMatch = strText.indexOf(strTarget);
        }

        return (strText);
    },
    replateHtmlEntities: function (msg) {
        if (msg.indexOf('script') >= 0 || msg.indexOf('javascript') >= 0)
            return '';
        msg = utils.ReplaceAll(msg, "<", "&lt;");
        msg = utils.ReplaceAll(msg, ">", "&gt;");
        return msg;
    },

    postData: function (url, param, callBack, failCallback, contentType, async) {
        try {
            contentType = !contentType ? 'application/json; charset=utf-8' : contentType;
            async = async == false ? false : true;
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(param),
                contentType: contentType,
                dataType: "json",
                cache: false,
                crossDomain: true,
                async: async,
                xhrFields: { withCredentials: true },
                success: function (data) {
                    if (typeof callBack === 'function')
                        callBack(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (typeof failCallback === 'function')
                        failCallback(xhr.responseText);
                }
            });
        }
        catch (err) { }
    },
    postDataValidate: function (url, param, _beforeSend, callBack, failCallback, contentType) {
        try {
            contentType = !contentType ? 'application/json; charset=utf-8' : contentType;
            $.ajax({
                beforeSend: _beforeSend,
                type: "POST",
                url: url,
                data: JSON.stringify(param),
                contentType: contentType,
                dataType: "json",
                cache: false,
                crossDomain: true,
                xhrFields: { withCredentials: true },
                success: function (data) {
                    if (typeof callBack === 'function')
                        callBack(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (typeof failCallback === 'function')
                        failCallback(xhr.responseText);
                }
            });
        }
        catch (err) { }
    },
    getData: function (url, param, callBack, failCallback, contentType, async) {
        try {
            contentType = !contentType ? 'application/json; charset=utf-8' : contentType;
            async = async == false ? false : true;
            $.ajax({
                type: "GET",
                url: url,
                data: param,
                contentType: contentType,
                dataType: "json",
                cache: false,
                crossDomain: true,
                async: async,
                xhrFields: { withCredentials: true },
                success: function (data) {
                    if (typeof callBack === 'function')
                        callBack(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (typeof failCallback === 'function')
                        failCallback(xhr.responseText);
                }
            });
        }
        catch (err) { }
    },
};