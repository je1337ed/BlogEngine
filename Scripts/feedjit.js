var FJ_mhstnm = 'fj0-14';
function FJ_isIE() { return navigator.userAgent.indexOf("MSIE") != -1; }

function FJ_getScrQ(pre) {
    var sc = document.getElementsByTagName('script');
    for (var i = 0; i < sc.length; i++) {
        if (sc[i].src.indexOf(pre) == 0) {
            var u = sc[i].src;
            return u.replace(/^[^\?]+\??/, '');
        }
    }
    return "";
}
function FJ_getQVar(q, v) {
    var vars = q.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == v) {
            return pair[1];
        }
    }
    return "";
}
function FJ_get_cHost() {
    if (!(location && location.protocol && location.host && /https?:/.test(location.protocol))) {
        return 'localhost';
    }
    var host = location.host.replace(/^www\./i, '');
    host = host.replace(/:\d+$/, '');
    host = host.replace(/^\./, '');
    host = host.replace(/\.$/, '');
    host = host.replace(/[^a-zA-Z0-9\.\-]+/g, '');
    return host ? host : 'localhost';
}

function FJ_getHostNum(cHost) {
    var tot = 0;
    for (var i = 0; i < cHost.length; i++) {
        tot += cHost.charCodeAt(i);
    }
    return tot % 100;
}
function FJ_createCookie(name, value, secs) {
    if (secs) {
        var date = new Date();
        date.setTime(date.getTime() + (secs * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}
function FJ_readCookie(name) {
    if (document.cookie && document.cookie.substr) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    } else {
        return null;
    }
}
function FJ_eraseCookie(name) {
    FJ_createCookie(name, "", -1);
}
function FJ_cookiesEnabled() {
    if (window['FJ_CEN'] == 'yes') {
        return true;
    } else if (window['FJ_CEN'] == 'no') {
        return false;
    } else {
        FJ_createCookie('_fjdet1', 'det', 10);
        if (FJ_readCookie('_fjdet1') == 'det') {
            window['FJ_CEN'] = 'yes';
            return true;
        } else {
            window['FJ_CEN'] = 'no';
            return false;
        }
    }
}
function FJ_ignoreMe(elem) {
    if (!FJ_cookiesEnabled()) {
        alert("You do not have cookies enabled which means Feedjit is already ignoring you.");
        return;
    }
    if (FJ_readCookie('_fjIgnoreMe') == '1') {
        alert("Feedjit is already ignoring you.");
        elem.innerHTML = 'Stop ignoring me';
        elem.onclick = function() { FJ_stopIgnoringMe(this); return false; };
        return;
    }
    FJ_createCookie('_fjIgnoreMe', '1', 86400 * 365);
    alert("From now on you will be ignored by Feedjit on this website.\n\nIf you use a different browser or workstation, or if you clear\nyour browser's cookies you will need to tell Feedjit to ignore you again.");
    elem.innerHTML = 'Stop ignoring me';
    elem.onclick = function() { FJ_stopIgnoringMe(this); return false; };
    return;
}
function FJ_stopIgnoringMe(elem) {
    if (!FJ_cookiesEnabled()) {
        alert("You do not have cookies enabled which means Feedjit has no choice but to ignore you.");
        return;
    }
    if (FJ_readCookie('_fjIgnoreMe') != '1') {
        alert("Feedjit is not currently ignoring you.");
        return;
    }
    FJ_eraseCookie('_fjIgnoreMe');
    alert("Feedjit has stopped ignoring you.");
    elem.innerHTML = 'Ignore my browser';
    elem.onclick = function() { FJ_ignoreMe(this); return false; };
    return;
}

function FJ_InjectCSS(href, storName) {
    var headID = document.getElementsByTagName("head")[0];
    var cssNode = document.createElement('link');
    cssNode.type = 'text/css';
    cssNode.rel = 'stylesheet';
    cssNode.href = href;
    cssNode.media = 'screen';
    if (typeof (window[storName]) != 'undefined' && window[storName] && window[storName].parentNode) {
        window[storName].parentNode.removeChild(window[storName]);
    }
    headID.appendChild(cssNode);
    window[storName] = cssNode;
}
function FJ_globalInit() {
    if (typeof (window['FJ_GINIT']) != 'undefined' && window['FJ_GINIT'] == 'run') {
        return;
    }
    window['FJ_GINIT'] = 'run';
    window['FJ_isNewVis'] = false;
    if (FJ_cookiesEnabled()) {
        var vid = FJ_readCookie('_fjvid1');
        if (vid) {
            window['FJ_VID'] = vid;
        } else {

            window['FJ_VID'] = 'new';
            window['FJ_isNewVis'] = true;
        }

        FJ_createCookie('_fjvid1', vid, 1800);
    } else {
        window['FJ_VID'] = 'cookiesDisabled';
    }
}
function FJ_setNewVID(vid) {
    window['FJ_VID'] = vid;
    FJ_createCookie('_fjvid1', vid, 1800);

}
function FJ_isNewVisit() {
    return window['FJ_isNewVis'] ? true : false;
}
function FJ_MakeWParams(wNum, wName, wProp) {
    var logRun = typeof (window['FJ_LogRun']) == 'undefined' ? 0 : 1;
    window['FJ_LogRun'] = true;
    var sw, sh;
    if (screen && screen.width && screen.height) {
        sw = screen.width;
        sh = screen.height;
    } else {
        sw = 0; sh = 0;
    }
    var ret =
	'w=' + wName +
	'&ign=' + ((FJ_readCookie('_fjIgnoreMe') == '1') ? '1' : '0') +
	'&wn=' + wNum +
	'&cen=' + (FJ_cookiesEnabled() ? '1' : '0') +
	'&nv=' + (FJ_isNewVisit() ? '1' : '0') +
	'&fl=' + (window['FJ_FIL'] ? '1' : '0') +
	'&vid=' + window['FJ_VID'] +
	'&rn=' + (typeof (window[wProp]) == 'undefined' ? '0' : '1') +
	'&lg=' + (logRun ? '0' : '1') +
	'&u=' + encodeURIComponent(location.href) +
	'&r=' + encodeURIComponent(document.referrer) +
	'&t=' + encodeURIComponent(document.title) +
	'&sw=' + sw +
	'&sh=' + sh +
	'&fjv=2' +
	'&rand=' + Math.floor(Math.random() * 999999999);
    window[wProp] = 1;
    return ret;
}
function FJ_Pause(millis) {
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while (curDate - date < millis);
}
function FJ_EDec(str) {
    var ta = document.createElement("textarea");
    ta.innerHTML = str.replace(/</g, "&lt;").replace(/>/g, "&gt;");
    return ta.value;
}
function FJ_shortenPre(str, chars) {
    if (str.length <= chars) return str;
    chars -= 3;
    str = str.substr(str.length - chars, chars);
    str = '...' + str.replace(/^[^\s\r\n\t]+[\s\r\n\t]+/, '');
    return str;
}
function FJ_shortenPost(str, chars) {
    if (str.length <= chars) return str;
    chars -= 3;
    str = str.substr(0, chars);
    str = str.replace(/[\s\r\n\t]+[^\s\r\n\t]+$/, '') + '...';
    return str;
}

function FJ_HandleAClick(aObj, evt) {
    evt = evt || window.event;
    //At this point we're gauranteed to be heading to a new location
    var href = aObj.href;

    var linkText = aObj.innerHTML ? FJ_EDec(aObj.innerHTML.replace(/<[^>]+>/g, '')) : '';
    var preText = "";
    var postText = "";

    var prevSib = aObj;
    while ((prevSib = prevSib.previousSibling) && preText.length < 255) {
        if (prevSib.nodeType == 3) {
            preText = FJ_EDec(prevSib.nodeValue) + preText;
        } else if (prevSib.nodeType == 1 && prevSib.firstChild && prevSib.firstChild === prevSib.lastChild && prevSib.firstChild.nodeType == 3) {
            preText = FJ_EDec(prevSib.firstChild.nodeValue) + preText;
        } else {
            break;
        }
    }
    var nextSib = aObj;
    while ((nextSib = nextSib.nextSibling) && postText.length < 255) {
        if (nextSib.nodeType == 3) {
            postText += FJ_EDec(nextSib.nodeValue);
        } else if (nextSib.nodeType == 1 && nextSib.firstChild && nextSib.firstChild === nextSib.lastChild && nextSib.firstChild.nodeType == 3) {
            postText += FJ_EDec(nextSib.firstChild.nodeValue);
        } else {
            break;
        }
    }
    linkText = linkText.replace(/[\s\r\n\t]+/g, ' ');
    preText = preText.replace(/[\s\r\n\t]+/g, ' ');
    postText = postText.replace(/[\s\r\n\t]+/g, ' ');

    linkText = FJ_shortenPost(linkText, 125);
    preText = FJ_shortenPre(preText, 125);
    postText = FJ_shortenPost(postText, 125);

    var img = document.createElement('IMG');
    img.width = '1';
    img.height = '1';
    img.alt = 'blah';
    img.src = 'http://feedjit.com/click/?' +
		'&h=' + encodeURIComponent(href) +
		'&u=' + encodeURIComponent(location.href) +
		'&cen=' + (FJ_cookiesEnabled() ? '1' : '0') +
		'&vid=' + window['FJ_VID'] +
		'&ign=' + ((FJ_readCookie('_fjIgnoreMe') == '1') ? '1' : '0') +
		'&fl=' + (window['FJ_FIL'] ? '1' : '0') +
		'&lnt=' + encodeURIComponent(linkText) +
		'&prt=' + encodeURIComponent(preText) +
		'&pot=' + encodeURIComponent(postText) +
		'&tfen=' + (window['FJ_TFEN'] ? '1' : '0') +
		'&fjv=2' +
		'&t=' + encodeURIComponent(document.title.replace(/[\s\r\n\t]+/g, ' ')) +
		'&rand=' + Math.floor(Math.random() * 999999999);
    document.body.appendChild(img);

    var img2 = document.createElement('IMG');
    img2.width = '1';
    img2.height = '1';
    img2.alt = '';
    img2.src = 'http://feedjit.com/images/transparent.gif?r=' + Math.floor(Math.random() * 999999999);
    document.body.appendChild(img2);
    FJ_Pause(500);
    return true;
}
function FJ_InstallATrack(a) {
    if (typeof (a.onclick) == 'function') {
        return; //Disable for now
        var old = a.onclick;
        a.onclick = function(e) {
            if (old(e)) {
                //If old() returns true then FJ_HandleAClick will handle taking us to the new location but will load an img first
                FJ_HandleAClick(this, e);
            }
            return false;
        };
    } else {
        a.onclick = function(e) { return FJ_HandleAClick(this, e) };
    }
}
function FJ_ProcessLinks() {
    var arr = document.body.getElementsByTagName('A');
    for (var i = 0; i < arr.length; i++) {
        if (/^(http:\/\/|\/)/.test(arr[i].href)) {
            FJ_InstallATrack(arr[i]);
        }
    }
}
function FJ_setupClickTrk() {
    if (typeof (window['FJClickTrkInst']) == 'undefined') {
        window['FJClickTrkInst'] = true;
        var oldonload = window.onload;
        if (typeof window.onload != 'function') {
            window.onload = FJ_ProcessLinks;
        } else {
            window.onload = function() {
                FJ_ProcessLinks();
                oldonload();
            }
        }
    }
}
FJ_setupClickTrk();


function FJTLSetup() {
    FJ_globalInit();
    var qstr = FJ_getScrQ('http://feedjit.com/serve');
    window['FJ_FIL'] = FJ_getQVar(qstr, 'fl') == 1 ? true : false;
    window['FJ_TFEN'] = true;
    FJ_InjectCSS('http://feedjit.com/style/serve/?' + qstr, 'FJ_TL_CSS');
    document.write('<script charset="utf-8" type="text/javascript" src="http://feedjit.com/router/?' +
		FJ_MakeWParams('1', 'trafficList', 'FJ_TL_RUN') + '"></script>');
}
FJTLSetup();

