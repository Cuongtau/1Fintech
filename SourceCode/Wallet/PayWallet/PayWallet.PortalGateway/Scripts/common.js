var extendData = '';
var VerificationToken = '';
var bankPaymentID = 0;
var bankCodePayment = '';
function GetCaptcha() {
    var UrlCaptcha = utils.rootUrl() + "api/Captcha/Get";
    $.ajax({
        type: 'GET',
        url: UrlCaptcha,
        success: function (captcha) {
            //Bind catpcha
            $('#inputToken').val(captcha.Verify);
            $('#imgCaptcha').attr('src', 'data:image/jpeg;base64,' + captcha.ImageData);
            //End bind catpcha
        },
        error: function () {
        }
    });
}

// Xử lý captcha
function RefreshCaptcha() {
    $.get(utils.rootUrl() + "/api/Captcha/Get").done(function (captcha) {
        $('#inputToken').val(captcha.Verify);
        $('#imgCaptcha').attr('src', 'data:image/jpeg;base64,' + captcha.ImageData);
    })
}


function browser() {
    if (browser.prototype._cachedResult)
        return browser.prototype._cachedResult;

    // Opera 8.0+ (UA detection to detect Blink/v8-powered Opera)
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';

    // At least Safari 3+: "[object HTMLElementConstructor]"
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;

    var isCocCoc = navigator.userAgent.indexOf('coc_coc') >= 0;

    // Chrome 1+
    var isChrome = !!window.chrome && !isOpera && !isCocCoc;

    // At least IE6
    var isIE = /*@cc_on!@*/false || !!document.documentMode;

    // Edge 20+
    //var isEdge = !isIE && !!window.StyleMedia;

    return browser.prototype._cachedResult =
        isOpera ? '11' :
            isFirefox ? '6' :
                isSafari ? '10' :
                    isCocCoc ? '9' :
                        isChrome ? '7' :
                            isIE ? '8' :
                                //isEdge ? 'Edge' :
                                "12";
};

function LocalStorageHelper(type, key, data) {
    try {
        if (typeof (Storage) === "undefined") return '';
        //set
        if (type === 0) {
            if (data === null || data === undefined || data === '') return;
            if (key === 'vtc_device_secure') {
                localStorage.setItem(key, CryptoJS.AES.encrypt(data, "4cab17a1134e44d298506398cafd5235"));
            }
            else {
                localStorage.setItem(key, data);
            }
        }
        //get
        else {
            if (key === 'vtc_device_secure') {
                var decrypted = CryptoJS.AES.decrypt(localStorage.getItem(key), "4cab17a1134e44d298506398cafd5235");
                return decrypted.toString(CryptoJS.enc.Utf8);
            }
            return localStorage.getItem(key);
        }
    } catch (e) {
        console.log(e.message);
        return '';
    }
}

//=========================================== Các hàm js xử lý lỗi, validate =========================================

// Show thông báo lỗi, highlight input, focus
function ShowErrorWithNotice(t, notice) {
    $(t).focus();
    $(t).closest("div.input-group").css({
        "border": "1px solid red",
        "border-radius": "4px"
    });
    $(t).parent().next('.errorNotice').html(notice);
}

// Show thông báo lỗi, highlight input, focus, remove style 1 element được truyền vào
function ShowErrorWithNotice(t, notice, that) {
    $(t).focus();
    $(t).closest("div.input-group").css({
        "border": "1px solid red",
        "border-radius": "4px"
    });
    $(t).parent().next('.errorNotice').html(notice);

    $(that).closest("div.input-group").css({
        "border": "none",
        "border-radius": "0"
    });
}

//Remove thông báo lỗi + normal input
function RemoveCaution(t) {
    $(t).closest("div.input-group").css({
        "border": "none",
        "border-radius": "0"
    });
    $(t).parent().next('.errorNotice').html("");
}

//Show thông báo lỗi + highlight input
function ShowErrorMessage(t, fieldid, text) {
    utils.clearErrorMessage(fieldid);
    $(t).closest("div.input-group").css({
        "border": "1px solid red",
        "border-radius": "4px"
    });
    $('#' + fieldid).html(text);
    $('#' + fieldid).show();
}

//Thanh toán ngân hàng
function CheckBankPayment(bankCode, bankType, extendData, bankInfo, lang) {
    if (bankCode === null || bankCode === undefined || bankCode === "") {
        utils.showPopup("Vui lòng chọn ngân hàng cần thanh toán");
        return;
    }
    else if (bankType === null || bankType === undefined || bankType === "") {
        utils.showPopup("Vui lòng chọn ngân hàng cần thanh toán");
        return;
    }
    if (extendData === '') {
        utils.showPopup("Yêu cầu không hợp lệ");
        return;
    }

    var parameters = {
        bankCode: bankCode,
        bankType: bankType,
        extendData: extendData,
        bankInfo: bankInfo
    };

    var urlRequestAns = utils.rootUrl() + "PayPortal/CheckPaygateOrderBank";
    utils.loading();
    $.ajax({
        type: 'POST',
        url: urlRequestAns,
        data: parameters,
        dataType: 'json',
        headers: {
            'VerificationToken': VerificationToken
        },
        success: function (data) {

            if (data.ResponseCode == -3333) {
                //var language = utils.getCurrentLanguage();
                var urlViewBank = utils.rootUrl() + "onlinebanking.html?bankCode=" + bankCode + "&extendData=" + extendData;
                if (lang.trim() === "en") {
                    urlViewBank += "&l=" + lang.trim();
                }
                window.location = urlViewBank;
            }
            else if (data.ResponseCode < 0) {
                utils.unLoading();
                var url = (data.Extend == null || data.Extend == "") ? '' : 'window.location="' + data.Extend + '"';
                if (url == "")
                    utils.showPopup(data.Description);
                else
                    utils.showPopupConfirm(data.Description, '', 2, url);
            } else {
                utils.unLoading();
                if (data.ResponseCode == 9000)
                    window.location = data.Extend;
                else if (data.ResponseCode == 1000) {
                    var urlPostData = utils.rootUrl() + "post-bank.html?data=" + data.Extend;
                    window.location = urlPostData;
                }
                else {
                    utils.unLoading();
                    utils.showPopup(data.Description);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            utils.unLoading();
            utils.showPopup('Hệ thống bận. Vui lòng thử lại sau');
        }
    });
}

function BankVerifyData(merchantCode, order_number, bankCode, accountHolder, cardNumber, amount, productDescription) {
    utils.translateLang('common.homepage');
    var param = {
        MerchantCode: merchantCode,
        TransactionID: order_number,
        BankCode: bankCode,
        BankFullName: accountHolder.toUpperCase(),
        BankCardNumber: cardNumber,
        Amount: amount,
        ProductDescription: productDescription
    };
    utils.loading();
    utils.postData(utils.portalUrl() + "PaymentByBank", param, function (data) {
        if (data.c >= 0) {
            utils.showPopupConfirmCodeBank(data.d.OrderID, data.d.Sign);
        }
        else {
            utils.unLoading();
            utils.showPopup(data.m);
        }
    },
        function (dataErr) {
            dataErr = JSON.parse(dataErr);
            utils.unLoading();
            var msg = "";
            switch (dataErr.c) {
                case -10145://số tiền thanh toán quá nhỏ
                    msg = i18n.t('payment.minAmountInvalid');
                    break;
                case -10103://Thông tin thẻ không hợp lệ
                    msg = i18n.t('payment.cardInfoInvalid');
                    break;
                case -100023://Lỗi tạo đơn hàng
                    msg = i18n.t('payment.createOrderErr');
                    break;
                case -10008://dữ liệu ko hợp lệ
                    msg = i18n.t('payment.dataInvalid');
                    break;
                case -100031://Kết nối đến ngân hàng lỗi
                    msg = i18n.t('payment.bankConnectErr');
                    break;
                default://
                    msg = i18n.t('payment.systemErr');
                    break;
            }
            utils.showPopup(msg);

        });
}

function ConfirmCodeBankDirect(orderId, sign) {
    utils.translateLang('common.homepage');
    $("div.input-group").css({
        "border": "none",
        "border-radius": "0"
    });
    $(".errorNotice").html("").hide();

    if (sign == '' || sign == null || orderId == '' || bankCodePayment == '' || cardNumber == '' || accountHolder == '' || merchantCode == '') {
        ShowErrorMessage(codeBank, "divSecureError", i18n.t('payment.requestInvalid'));
        return;
    }
    var codeBank = $("#inp_code").val();
    if (codeBank.trim() == "") {
        ShowErrorMessage(codeBank, "divSecureError", i18n.t('payment.inputOTPPayment'));
        return;
    }
    var param = {
        BankCode: bankCodePayment,
        BankCardNumber: cardNumber,
        BankFullName: accountHolder.toUpperCase(),
        MerchantCode: merchantCode,
        OrderID: orderId,
        OTP: codeBank,
        VerifySign: sign
    };
    utils.loading();
    utils.postData(utils.portalUrl() + "PaymentByBankConfirm", param, function (data) {
        console.log(data);
        debugger;
        utils.unLoading();
        var url = (data.d.urlRedirect == null || data.d.urlRedirect == "") ? '' : data.d.urlRedirect;
        if (data.c >= 0) {
            utils.showPopupConfirm("", i18n.t('homepage.IKnew'), 1, 'window.location="' + url + '"');
            if (url != "") {
                setTimeout(function () {
                    window.location = url;
                },5000);
            }
        }
        else
            utils.showPopupConfirm(data.m, i18n.t('homepage.Retry'), 2, 'window.location="' + url + '"');
    },
        function (data) {
            console.log(data);
            debugger;
            data = JSON.parse(data);
            utils.unLoading();
            var url = (data.d.urlRedirect == undefined || data.d.urlRedirect == null || data.d.urlRedirect == "") ? '' : data.d.urlRedirect;
            var msg = "";
            switch (data.c) {
                case -10145://số tiền thanh toán quá nhỏ
                    msg = i18n.t('payment.minAmountInvalid');
                    break;
                case -10103://Thông tin thẻ không hợp lệ
                    msg = i18n.t('payment.cardInfoInvalid');
                    break;
                case -100023://Lỗi tạo đơn hàng
                    msg = i18n.t('payment.createOrderErr');
                    break;
                case -10008://dữ liệu ko hợp lệ
                    msg = i18n.t('payment.dataInvalid');
                    break;
                case -100031://Kết nối đến ngân hàng lỗi
                    msg = i18n.t('payment.bankConnectErr');
                    break;
                case -100999://Gd đang chờ xử lý
                    msg = i18n.t('payment.transactionReview');
                    break;
                case -51://số dư không đủ
                    msg = i18n.t('payment.notEnoughAmount');
                    break;
                case -10104://thẻ chưa đký IBanking
                    msg = i18n.t('payment.notRegisterIBanking');
                    break;
                case -10153://Giao dịch vượt quá hạn mức cho phép
                    msg = i18n.t('payment.overAmountLimit');
                    break;
                case -10015: // Sai OTP
                    msg = i18n.t('payment.errorOTP');
                    $("#inp_code").val('').focus();
                    break;
                default://
                    msg = i18n.t('payment.systemErr');
                    break;
            }
            if (url != '') {
                utils.showPopupConfirm(msg, i18n.t('homepage.Retry'), 2, 'window.location="' + url + '"');
                setTimeout(function () {
                    window.location = url;
                }, 5000);
            }
            else if (data.c == -10008 || data.c == -10015 || data.c == -6) {
                ShowErrorMessage(codeBank, "divSecureError", msg);
                $("#inp_code").val('').focus();
                return;
            }
            else
                utils.showPopup(msg);
        });
}
function PayByWalletLink(bankID, bankCode, extendData) {
    var param = {
        bankID: bankID,
        bankCode: bankCode,
        extendData: extendData
    };
    utils.loading();
    $.ajax({
        type: 'POST',
        url: utils.rootUrl() + "PayPortal/PayByWalletLink",
        data: param,
        dataType: 'json',
        headers: {
            'VerificationToken': VerificationToken
        },
        success: function (data) {
            var url = (data.Extend == null || data.Extend == "") ? "" : 'window.location="' + data.Extend + '"';
            utils.unLoading();
            if (data.ResponseCode == -508)
                utils.showPopupConfirmWalletLink(data.Extend);
            else if (data.ResponseCode > 0)
                utils.showPopupConfirm(data.Description, i18n.t('homepage.IKnew'), 1, url);
            else
                utils.showPopupConfirm(data.Description, i18n.t('homepage.Retry'), 2, url);

        },
        error: function () {
            utils.unLoading();
            utils.showPopup("Hệ thống đang bân. Vui lòng thử lại sau !");
        }
    });
}
function ResendODP() {
    utils.loading();
    var param = {
        extendData: dataExt
    };
    $.ajax({
        type: 'POST',
        url: utils.rootUrl() + "PayPortal/ResendODP",
        data: param,
        dataType: 'json',
        headers: {
            'VerificationToken': VerificationToken
        },
        success: function (data) {
            utils.unLoading();
            if (data.ResponseCode > 0) {
                utils.showPopup(data.Description);
            }
            else {
                utils.showPopup(data.Description);
            }
        },
        error: function () {
            utils.unLoading();
            utils.showPopup("Hệ thống đang bận, Vui lòng thử lại sau !");
        }
    });
};



function FormatCurrency(ctrl) {
    //Check if arrow keys are pressed - we want to allow navigation around textbox using arrow keys
    if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
        return;
    }
    var val = ctrl.value;
    val = val.replace(/[.,]/g, "")
    ctrl.value = "";
    val += '';
    x = val.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';

    var rgx = /(\d+)(\d{3})/;

    //custom dot (.) or comma(,) here
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }

    ctrl.value = x1 + x2;
}
function changeInput(t, e) {
    e.preventDefault();
    if (e.which != 13) {
        if ($(".errorNotice").is(":visible")) {
            $("div.input-group").css({
                "border": "none",
                "border-radius": "0"
            });
            $(".errorNotice").html("").hide();
        }
    }
}


function downloadFile(filename, data) {
    var DownloadEvt = null;
    var DownloadLink = document.createElement('a');

    if (DownloadLink) {
        DownloadLink.style.display = 'none';
        DownloadLink.download = filename;
        DownloadLink.href = data;

        document.body.appendChild(DownloadLink);

        if (document.createEvent) {
            if (DownloadEvt == null)
                DownloadEvt = document.createEvent('MouseEvents');

            DownloadEvt.initEvent('click', true, false);
            DownloadLink.dispatchEvent(DownloadEvt);
        }
        else if (document.createEventObject)
            DownloadLink.fireEvent('onclick');
        else if (typeof DownloadLink.onclick == 'function')
            DownloadLink.onclick();

        document.body.removeChild(DownloadLink);
    }
}
function utf8Encode(string) {
    string = string.replace(/\x0d\x0a/g, "\x0a");
    var utftext = "";
    for (var n = 0; n < string.length; n++) {
        var c = string.charCodeAt(n);
        if (c < 128) {
            utftext += String.fromCharCode(c);
        }
        else if ((c > 127) && (c < 2048)) {
            utftext += String.fromCharCode((c >> 6) | 192);
            utftext += String.fromCharCode((c & 63) | 128);
        }
        else {
            utftext += String.fromCharCode((c >> 12) | 224);
            utftext += String.fromCharCode(((c >> 6) & 63) | 128);
            utftext += String.fromCharCode((c & 63) | 128);
        }
    }
    return utftext;
}
function base64encode(input) {
    var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    var output = "";
    var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
    var i = 0;
    input = utf8Encode(input);
    while (i < input.length) {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output = output +
            keyStr.charAt(enc1) + keyStr.charAt(enc2) +
            keyStr.charAt(enc3) + keyStr.charAt(enc4);
    }
    return output;
}
var saveAs = saveAs || function (e) { "use strict"; if (typeof navigator !== "undefined" && /MSIE [1-9]\./.test(navigator.userAgent)) { return } var t = e.document, n = function () { return e.URL || e.webkitURL || e }, r = t.createElementNS("http://www.w3.org/1999/xhtml", "a"), i = "download" in r, o = function (e) { var t = new MouseEvent("click"); e.dispatchEvent(t) }, a = /Version\/[\d\.]+.*Safari/.test(navigator.userAgent), f = e.webkitRequestFileSystem, u = e.requestFileSystem || f || e.mozRequestFileSystem, s = function (t) { (e.setImmediate || e.setTimeout)(function () { throw t }, 0) }, c = "application/octet-stream", d = 0, l = 500, w = function (t) { var r = function () { if (typeof t === "string") { n().revokeObjectURL(t) } else { t.remove() } }; if (e.chrome) { r() } else { setTimeout(r, l) } }, p = function (e, t, n) { t = [].concat(t); var r = t.length; while (r--) { var i = e["on" + t[r]]; if (typeof i === "function") { try { i.call(e, n || e) } catch (o) { s(o) } } } }, v = function (e) { if (/^\s*(?:text\/\S*|application\/xml|\S*\/\S*\+xml)\s*;.*charset\s*=\s*utf-8/i.test(e.type)) { return new Blob(["\ufeff", e], { type: e.type }) } return e }, y = function (t, s, l) { if (!l) { t = v(t) } var y = this, m = t.type, S = false, h, R, O = function () { p(y, "writestart progress write writeend".split(" ")) }, g = function () { if (R && a && typeof FileReader !== "undefined") { var r = new FileReader; r.onloadend = function () { var e = r.result; R.location.href = "data:attachment/file" + e.slice(e.search(/[,;]/)); y.readyState = y.DONE; O() }; r.readAsDataURL(t); y.readyState = y.INIT; return } if (S || !h) { h = n().createObjectURL(t) } if (R) { R.location.href = h } else { var i = e.open(h, "_blank"); if (i == undefined && a) { e.location.href = h } } y.readyState = y.DONE; O(); w(h) }, b = function (e) { return function () { if (y.readyState !== y.DONE) { return e.apply(this, arguments) } } }, E = { create: true, exclusive: false }, N; y.readyState = y.INIT; if (!s) { s = "download" } if (i) { h = n().createObjectURL(t); r.href = h; r.download = s; setTimeout(function () { o(r); O(); w(h); y.readyState = y.DONE }); return } if (e.chrome && m && m !== c) { N = t.slice || t.webkitSlice; t = N.call(t, 0, t.size, c); S = true } if (f && s !== "download") { s += ".download" } if (m === c || f) { R = e } if (!u) { g(); return } d += t.size; u(e.TEMPORARY, d, b(function (e) { e.root.getDirectory("saved", E, b(function (e) { var n = function () { e.getFile(s, E, b(function (e) { e.createWriter(b(function (n) { n.onwriteend = function (t) { R.location.href = e.toURL(); y.readyState = y.DONE; p(y, "writeend", t); w(e) }; n.onerror = function () { var e = n.error; if (e.code !== e.ABORT_ERR) { g() } }; "writestart progress write abort".split(" ").forEach(function (e) { n["on" + e] = y["on" + e] }); n.write(t); y.abort = function () { n.abort(); y.readyState = y.DONE }; y.readyState = y.WRITING }), g) }), g) }; e.getFile(s, { create: false }, b(function (e) { e.remove(); n() }), b(function (e) { if (e.code === e.NOT_FOUND_ERR) { n() } else { g() } })) }), g) }), g) }, m = y.prototype, S = function (e, t, n) { return new y(e, t, n) }; if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) { return function (e, t, n) { if (!n) { e = v(e) } return navigator.msSaveOrOpenBlob(e, t || "download") } } m.abort = function () { var e = this; e.readyState = e.DONE; p(e, "abort") }; m.readyState = m.INIT = 0; m.WRITING = 1; m.DONE = 2; m.error = m.onwritestart = m.onprogress = m.onwrite = m.onabort = m.onerror = m.onwriteend = null; return S }(typeof self !== "undefined" && self || typeof window !== "undefined" && window || this.content); if (typeof module !== "undefined" && module.exports) { module.exports.saveAs = saveAs } else if (typeof define !== "undefined" && define !== null && define.amd != null) {
    define([],
        function () { return saveAs; });
}

function ExportPDF(type) {
    // playground requires you to assign document definition to a variable called dd
    var dd = {
        content: [
            {
                table: {
                    headerRows: 1,
                    widths: '*',
                    body: [
                        [{ text: 'ELECTRONIC INVOICE', style: 'header' }]
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 3 || i === node.table.body.length) ? 1 : 0;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 0 : 0;
                    },
                    hLineColor: function (i, node) {
                        return 'gray';
                    },
                    vLineColor: function (i, node) {
                        return 'gray';
                    },
                }
            },
            '\n\n',
            {
                columns: [
                    {
                        width: '40%',
                        table: {
                            widths: '*',
                            body: [
                                [{ text: $("#merchantName").text(), style: 'tableBodyTop' }],
                                [{ text: $("#address").text(), style: 'tableBodyTop' }],
                                [{ text: $("#email").text(), style: 'tableBodyTop', color: '#3678d5', decoration: 'underline' }],
                                [{ text: $("#mobile").text(), style: 'tableBodyTop' }],

                            ]
                        },
                        layout: 'noBorders'
                    },
                    {
                        table: {
                            widths: [150, 100, 70, '*'],
                            body: [
                                [{ text: 'Amount Due', style: 'tableHeader' }, { text: $("#amountOrder").text(), style: 'tableHeader' }],
                                [{ text: 'Invoice Date', style: 'tableBodyTop' }, { text: $("#createOrder").text(), style: 'tableBodyTop' }],
                                [{ text: 'Order Code', style: 'tableBodyTop' }, { text: $("#transactionId").text(), style: 'tableBodyTop' }],
                                [{ text: 'Transaction Time', style: 'tableBodyTop' }, { text: $("#createOrder").text(), style: 'tableBodyTop' }],
                                [{ text: 'Payment type', style: 'tableBodyTop' }, { text: $("#payType").text(), style: 'tableBodyTop' }],
                                [{ text: 'Status', style: 'tableBodyTop' }, { text: $("#statusOrder").text(), style: 'tableBodyTop' }]
                            ]
                        },
                        layout: 'noBorders'
                    }
                ]
            },
            '\n\n',
            {

                table: {
                    dontBreakRows: true,
                    headerRows: 1,
                    widths: [120, 130, 50, '*', '*'],
                    body: [
                        [{ text: 'Product', style: 'tableHeader' }, { text: 'Product Description', style: 'tableHeader' }, { text: 'Quantity', style: 'tableHeader' }, { text: 'Unit Price', style: 'tableHeader' }, { text: 'Amount', style: 'tableHeader' }],
                        [{ text: $("#productName").text(), style: 'tableBody' }, { text: $("#desc").text(), style: 'tableBody' }, { text: $("#quantity").text(), style: 'tableBody' }, { text: $("#price").text(), style: 'tableBody' }, { text: $("#totalAmount").text(), style: 'tableBody' }],

                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 3 || i === node.table.body.length) ? 1 : 0;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 0 : 0;
                    },
                    hLineColor: function (i, node) {
                        return 'gray';
                    },
                    vLineColor: function (i, node) {
                        return 'gray';
                    },
                }
            },
            {
                table: {
                    dontBreakRows: true,
                    headerRows: 1,
                    widths: [120, 130, 50, '*', '*'],
                    body: [
                        [{ text: '', style: 'tableBody' }, { text: '', style: 'tableBody' }, { colSpan: 2, text: 'Subtotal', style: 'tableBody' }, {}, { text: $("#totalAmount").text(), style: 'tableBody' }],
                        [{ text: '', style: 'tableBody' }, { text: '', style: 'tableBody' }, { colSpan: 2, text: 'Tax', style: 'tableBody' }, {}, { text: $("#totalFee").text(), style: 'tableBody' }],
                        [{ text: '', style: 'tableFooter' }, { text: '', style: 'tableFooter' }, { colSpan: 2, text: 'Total Invoice', style: 'tableFooter' }, {}, { text: $("#totalAmountPayment").text(), style: 'tableFooter' }],
                    ]
                },
                layout: 'noBorders'
            },
            '\n\n',
            {
                text: 'Order processed securely by VTC Pay E-Payment Gateway\n Thanks for using our services! \n Hotline: 19001530 - hotro.vtc.vn',
                alignment: "center",
                color: '#1a1a1a'
            },
            '\n',
            {
                text: 'Note: Payment process completed, Please wait for being shipped.',
                alignment: "center",
                color: '#1a1a1a',
                fontSize: 11,
                italics: true
            },
            '\n',
            {
                table: {
                    headerRows: 1,
                    widths: '*',
                    body: [
                        [{ text: '', style: 'header' }]
                    ]
                },
                layout: {
                    hLineWidth: function (i, node) {
                        return (i === 3 || i === node.table.body.length) ? 1 : 0;
                    },
                    vLineWidth: function (i, node) {
                        return (i === 0 || i === node.table.widths.length) ? 0 : 0;
                    },
                    hLineColor: function (i, node) {
                        return '#e1e1e1';
                    },
                    vLineColor: function (i, node) {
                        return '#e1e1e1';
                    },
                }

            }
        ],
        styles: {
            header: {
                fontSize: 18,
                bold: true,
                color: '#0061af',
                margin: [0, 10]
            },
            bigger: {
                fontSize: 15,
                italics: true,
            },
            tableBodyTop: {
                color: '#333333',
                margin: [5, 3, 5, 3]
            },
            tableBody: {
                color: '#333333',
                margin: [5, 5, 5, 5]
            },
            tableHeader: {
                bold: true,
                fontSize: 13,
                color: 'white',
                fillColor: "#4fb4eb",
                align: 'center',
                margin: [4, 3]
            },
            tableFooter: {
                bold: true,
                fontSize: 13,
                color: 'white',
                fillColor: "#4fb4eb",
                align: 'center',
                margin: [4, 3]
            }
        },
        defaultStyle: {
            columnGap: 20,
        },
        pageSize: "A4",
        pageOrientation: "portrait",
        info: {
            title: 'Electronic Invoice',
            author: 'VTCPay',
            subject: 'Electronic Invoice',
            keywords: 'Payment, wallet, VTC Pay',
        },
    };
    if (type == 1) {
        pdfMake.createPdf(dd).download('electronic-invoice.pdf');
    }
    else
        pdfMake.createPdf(dd).print();
    // open the PDF in a new window
    //pdfMake.createPdf(dd).open();

}
function exportDoc() {
    var MSDocType = 'word';
    var MSDocExt = 'doc';
    var MSDocSchema = 'xmlns:w="urn:schemas-microsoft-com:office:word"';
    debugger
    var docData = "<div class='duoi mg-top-20'> <h3 style='color: #0061af;font-size: 22px;font-weight: bold;text-transform: uppercase; border-bottom: 1px solid #e1e1e1;padding-bottom: 15px;margin-bottom: 30px'>Electronic Invoice</h3><div class='content'>";

    docData += "<div style='margin-bottom:30px;float:left;width:100%'><table><tbody><tr><td><table width='300' border='0'><tbody><tr><td><p>" + $("#merchantName").text() + "</p></td></tr>";
    docData += "<tr><td><p>" + $("#address").text() + "</p></td></tr>" + "<tr><td><p><a>" + $("#email").text() + "</a></p></td></tr>" + "<tr><td><p>" + $("#mobile").text() + "</p></td></tr></tbody></table></td><td>";
    docData += $(".col-lg-7").html();
    docData += "</td></tr></tbody></table></div>";
    docData += $(".table-responsive").html();
    docData += "<div style='margin-top:50px;'>" + $(".mg-bt-25").html() + "</div>";
    docData += "</div></div>";
    var docFile = '<html xmlns:o="urn:schemas-microsoft-com:office:office" ' + MSDocSchema + ' xmlns="http://www.w3.org/TR/REC-html40">';
    docFile += '<meta http-equiv="content-type" content="application/vnd.ms-' + MSDocType + '; charset=UTF-8">';
    docFile += "<head><style>"
        + "p {margin: 0 0 10px;}"
        + "#email a {color: #3678d5;text-decoration: underline;}"
        + "table.sm-table-right tr.first, tr.fillcolor, tr.add-bgtable {background-color: rgb(79, 180, 235);}"
        + "td.add-boldtable,.fillcolor td {font-weight: 600;color: #fff;}"
        + "tr.add-border {border-top: 1px solid #e1e1e1;}"
        + "tr td {padding: 5px;font-weight: 400;}"
        + "table.table-hoadon-full {width:100%; margin-top:30px; margin-bottom:30px}"
        + "center {padding-bottom:50px;border-bottom:1px solid #e1e1e1}"
        + "</style>";
    docFile += "</head>";
    docFile += "<body>";
    docFile += docData;
    docFile += "</body>";
    docFile += "</html>";
    var base64data = base64encode(docFile);
    try {
        var blob = new Blob([docFile], { type: 'application/vnd.ms-' + MSDocType });
        saveAs(blob, 'electronic-invoice.' + MSDocExt);
    }
    catch (e) {
        downloadFile(c.filename + '.' + MSDocExt, 'data:application/vnd.ms-' + MSDocType + ';base64,' + base64data);
    }
}