'use strict';

hui('.header .btn-left').click(function () {
    var drop = hui('.dropdown');
    var child = drop.find('a').length;
    if (drop.hasClass('open')) {
        drop.css({ height: '0rem' });
        drop.removeClass('open');
    } else {
        drop.css({ height: child * 1 + 'rem' });
        drop.addClass('open');
    }
});

function isIphoneX() {
    return (/iphone/gi.test(navigator.userAgent) && screen.height == 812 && screen.width == 375
    );
}
if (isIphoneX()) {
    console.log(123);
    hui('.footer').addClass('iphonex');
}

var utils = {};
utils.msg = function (title, text, btn, cb) {
    var tpl = '<div id="hui-dialog" class="">\n            <div id="hui-dialog-in">\n                <div id="hui-dialog-title">\n                    ' + title + '\n                </div>\n                <div id="hui-dialog-msg" style="padding-bottom:12px;">' + text + '\n                </div>\n                <div style="height:15px;"></div>\n                <div id="hui-dialog-btn-text">\n                ' + btn + '\n                </div>\n            </div>\n        </div>\n        <div id="hui-mask"></div>';
    var html = document.createElement('div');
    html.innerHTML = tpl;
    document.body.appendChild(html);
    var button = document.querySelector('#hui-dialog-btn-text');
    button.onclick = function () {
        var dialog = document.getElementById("hui-dialog");
        dialog.parentNode.removeChild(dialog);
        var mask = document.getElementById("hui-mask");
        mask.parentNode.removeChild(mask);
        if (cb) {
            cb();
        }
    };
};