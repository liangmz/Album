﻿
sessionStorage.page = 1;
sessionStorage.pageCount = -1;

if (document.all) {
    window.attachEvent('onload', init);
}
else {
    window.addEventListener('load', init);
}

function init() {
    if (document.getElementById('userName').innerHTML == '') {
        document.getElementById('fat-menu').style.display = 'none';
        document.getElementById('login').style.display = 'block';
        document.getElementById('register').style.display = 'block';
        document.getElementById('uname').value = window.localStorage['uname'] ? window.localStorage['uname'] : '';
        document.getElementById('loginCheckBox').checked = window.localStorage['uname'] ? true : false;
    }
    else {
        document.getElementById('login').style.display = 'none';
        document.getElementById('register').style.display = 'none';
        document.getElementById('fat-menu').style.display = 'block';
    }
    //登陆模态框退出
    $('#Login').on('hidden.bs.modal', function (e) {
        document.getElementById('password').value = '';
    });
}

//xmlhttprequest发送
function DoAjax(data, fun, url, mode) {
    var xhr = getXmlHttpRequest();
    xhr.open(mode, url, true);
    xhr.send(data);
    xhr.onreadystatechange = xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            fun(xhr.responseText);
            xhr = null;
        }
    }
}

/*登陆*/
function Login() {
    var uname = document.getElementById('uname');
    var password = document.getElementById("password");
    var checkBox = document.getElementById('loginCheckBox');

    if (window.localStorage && checkBox.checked) {
        window.localStorage['uname'] = uname.value;
    } else if (window.localStorage && !checkBox.checked) {
        window.localStorage.removeItem('uname');
    }

    if (uname.value.length <= 0 || password.value.length <= 0) {
        uname.parentElement.className += ' has-error';
        password.parentElement.className += ' has-error';
        return false;
    }
    var userName = document.getElementById('userName');
    var f_login = document.getElementById("f_login");
    var data = new FormData();
    data.append("userName", uname.value);
    data.append("password", password.value);

    var xhr_login = getXmlHttpRequest();
    xhr_login.open("post", "Ashx/Login.ashx", true);
    xhr_login.onreadystatechange = function () {
        if (xhr_login.readyState == 4 && xhr_login.status == 200) {
            if (xhr_login.responseText != '-1') {
                var barInfo = document.getElementById('barInfo');
                barInfo.style.width = '100%';
                userName.innerHTML = xhr_login.responseText + ' <b class="caret"></b>';
                init();
                f_login.reset();
                $('#Login').modal('hide');
                if (confirm("重新加载页面吗?")) {
                    location.reload();
                }
            }
            else {
                uname.parentElement.className += ' has-error';
                password.parentElement.className += ' has-error';
                password.value = '';
                
            }
            xhr_login = null;
            return false;
        }
        else {
            var barInfo = document.getElementById('barInfo');
            var state = xhr_login.readyState;
            barInfo.style.width = (Number(state) + Number(1)) * 25 + '%';
            return false;
        }
    }
    xhr_login.send(data);
    var barInfo = document.getElementById('barInfo');
    barInfo.style.width = '25%';
    return false;
}
/*下线*/
function LoginOut() {
    var xmlhttprequest = getXmlHttpRequest();
    xmlhttprequest.open("post", "Ashx/LoginOut.ashx", true);
    xmlhttprequest.onreadystatechange = function () {
        if (xmlhttprequest.readyState == 4) {
            document.getElementById("userName").innerHTML = "";
            //location.replace('Default.aspx');
            xmlhttprequest = null;
            location.reload();
        }
    }
    xmlhttprequest.send(null);
}


/*现在的时间*/
function getNow() {
    var time = new Date();
    document.write(time.toString());
}


/*获得异步请求*/
function getXmlHttpRequest() {
    var request;
    if (window.XMLHttpRequest)
        request = new XMLHttpRequest();
    else
        try {
            request = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {
            request = new ActiveXObject("Msxml2.XMLHTTP");
        };
    return request;
}

/*注册*/
function getRegister() {
    var vPic = document.getElementById("vPic");
    var xmlhttprequest = getXmlHttpRequest();
    xmlhttprequest.open("POST", "Ashx/ValiPic.ashx", true);
    xmlhttprequest.onreadystatechange = function () {
        if (xmlhttprequest.readyState == 4 && xmlhttprequest.status == 200) {
            vPic.src = 'data:image/jpeg;base64,' + xmlhttprequest.responseText;
            xmlhttprequest = null;
            $('#Register').modal('show');
           
        }
    }
    xmlhttprequest.send(null);

}

//验证码
function changeValiPic() {
    var vPic = document.getElementById("vPic");
    var xmlhttprequest = getXmlHttpRequest();
    xmlhttprequest.open("POST", "Ashx/ValiPic.ashx", true);
    xmlhttprequest.onreadystatechange = function () {
        if (xmlhttprequest.readyState == 4 && xmlhttprequest.status == 200) {
            vPic.src = 'data:image/jpeg;base64,' + xmlhttprequest.responseText;
            xmlhttprequest = null;
        }
    }
    xmlhttprequest.send(null);
}

function register() {
    var password = document.getElementById("r_password").value;
    var password2 = document.getElementById("r_password2").value;

    if (password.length <= 0 || password2.length <= 0 || password != password2) {
        document.getElementById('f_register').childNodes[2 * 1 + 1].className += ' has-error';
        document.getElementById('f_register').childNodes[2 * 2 + 1].className += ' has-error';
        return false;
    }
    else {
        document.getElementById('f_register').childNodes[2 * 1 + 1].className = 'form-group';
        document.getElementById('f_register').childNodes[2 * 2 + 1].className = 'form-group';
    }

    var email = document.getElementById("r_email").value;
    var code = document.getElementById("r_code").value;
    var userName = document.getElementById("usname").value;

    if (email.length <= 0 || code.length <= 0 || userName.length <= 0) {
        return false;
    }

    var f_register = document.getElementById("f_register");
    var data = new FormData(f_register);
    data.append("userName", userName);
    data.append("password", password);
    data.append("email", email);
    data.append("code", code);

    var xhr_r = getXmlHttpRequest();
    xhr_r.open("post", "Ashx/Register.ashx", true);
    xhr_r.onreadystatechange = function () {
        if (xhr_r.readyState == 4 && xhr_r.status == 200) {
            var result = xhr_r.responseText;
            xhr_r = null;
            FormClassReset();
            switch (result) {
                case '-1': {
                    document.getElementById('f_register').childNodes[2 * 4 + 1].className += ' has-error';
                    changeValiPic();
                } break;
                case '-2': {
                    document.getElementById('f_register').childNodes[2 * 0 + 1].className += ' has-error';
                } break;
                case '-3': {
                    document.getElementById('f_register').childNodes[2 * 3 + 1].className += ' has-error';
                } break;
                case '-4': {
                    $('#Lose').modal('show');
                } break;
                default: {
                    $('#Register').modal('hide');
                    $('#Success').modal('show'); 
                } break;
            }
        }
    }
    xhr_r.send(data);
    return false;
}

function FormClassReset()
{
    for (var i = 0 ; i <= 4 ; i++)
    {
        document.getElementById('f_register').childNodes[2 * i + 1].className = 'form-group';
    }
}

/*检查搜索关键字*/
function checkKeyword(searchGo, e) {
    if (document.search.keyword.value.length <= 0) {
        alert("Nothing");
        return false;
    }
    if (searchGo.getAttribute('value') == '0') {
        e.setAttribute('action', 'Search.aspx');
    }
    else {
        e.setAttribute('action', 'SearchPic.aspx');
    }
    return true;
}
//搜索内容种类选择
function buttonDownMenuAction(e, Go, Text) {
    Go.setAttribute('value', e.getAttribute('value'));
    Text.innerHTML = e.innerHTML + '<span class="caret"></span>';
}

/*分页函数*/
function changePage(ele) {
    var tpage = ele.innerHTML;//跳转到这个页面
    var UrlLink = ele.getAttribute('onclick');

    if (tpage == '...') {
        tpage = ele.getAttribute('page');
    }
    //ele.innerHTML = '<span class="glyphicon glyphicon-refresh tagLoading"></span>';
    try {
        changePageActive(tpage);
        buildPage(sessionStorage.pageCount, tpage, 'return changePage(this)');
        sessionStorage.page = tpage;
    } catch (e)
    { }
    return false;
}
/*-分页函数*/


/*建立分页栏*/
function buildPage(_pageCount, nowPage, ajaxUrl) {
    var pageLengthMax = 10;//页号栏长度

    /*页号区间确定*/
    var begin = 1;
    var end = _pageCount;
    if (_pageCount == '-1') {
        end = 1;
    }
    else if (_pageCount <= pageLengthMax) {
        end = _pageCount;
    } else {
        if (nowPage - 1 < pageLengthMax) {
            end = pageLengthMax;
        }
        else if (sessionStorage.pageCount - nowPage < pageLengthMax) {
            begin = sessionStorage.pageCount - pageLengthMax + 1;
        }
        else {
            begin = Number(nowPage) - Number(pageLengthMax / 2);
            end = Number(nowPage) + Number(pageLengthMax / 2) - 1;
            if (begin < 1) {
                begin = 1;
                end = _pageCount > pageLengthMax ? pageLengthMax : _pageCount;
            }
            else if (end > _pageCount) {
                begin = _pageCount - pageLengthMax > 0 ? Number(_pageCount) - Number(pageLengthMax) + Number(1) : 1;
                end = _pageCount;
            }
        }
    }
    /**/
    //清空子节点
    var pageE = document.getElementById('page');
    pageE.innerHTML = '';

    if (end == '1') return;

    //«子节点
    var pre = document.createElement('li');
    var pre_a = document.createElement('a');
    pre_a.innerHTML = '«';
    pre.appendChild(pre_a);
    pageE.appendChild(pre);

    if (begin > 2) {
        pageE.appendChild(getPagePoint_a(1, ajaxUrl));
        pageE.appendChild(getPagePoint_Un(Number(begin) - 1, ajaxUrl));
    }
    else if (begin != 1) {
        pageE.appendChild(getPagePoint_a(Number(begin) - 1, ajaxUrl));
    }

    for (var i = begin ; i <= end ; i++) {
        if (i == nowPage) {
            pageE.appendChild(getPagePoint_active(i));
        }
        else {
            pageE.appendChild(getPagePoint_a(i, ajaxUrl));
        }
    }
    //...
    if (_pageCount > end) {
        pageE.appendChild(getPagePoint_Un(Number(end) + 1, ajaxUrl));
    }
    //最后一个
    if (_pageCount - end > 1) {
        pageE.appendChild(getPagePoint_a(_pageCount, ajaxUrl));
    }

    //»子节点
    var next = document.createElement('li');
    var next_a = document.createElement('a');
    next_a.innerHTML = '»';
    next.appendChild(next_a);
    pageE.appendChild(next);
}

/*获取页面按钮（显示...）*/
function getPagePoint_Un(_page, ajaxUrl) {
    var li = document.createElement('li');
    var a = document.createElement('a');
    a.href = 'javascipt:void(0);';
    a.setAttribute('onclick', ajaxUrl);
    a.innerHTML = '...';
    a.setAttribute('page', _page);
    li.appendChild(a);

    return li;
}
/*获取页面按钮（非选中状态）*/
function getPagePoint_a(_page, ajaxUrl) {
    var li = document.createElement('li');
    var a = document.createElement('a');
    a.href = 'mailto:#';
    a.setAttribute('onclick', ajaxUrl);
    a.innerHTML = _page;
    li.appendChild(a);

    return li;
}
/*获取页面按钮（选中状态）*/
function getPagePoint_active(_page) {
    var li = document.createElement('li');
    li.className = 'active';
    var span = document.createElement('span');
    span.innerHTML = _page;
    var _span = document.createElement('span');
    _span.className = 'sr-only';
    _span.innerHTML = '(current)';
    span.appendChild(_span);
    li.appendChild(span);
    return li;
}

/*获得xml解析*/
function getXML(xml) {
    var xmlDoc;
    if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.loadXML(xml);
    }
    else {
        xmlDoc = new DOMParser().parseFromString(xml, "text/xml");
    }
    return xmlDoc;
}


function ValiEmail() {
    document.getElementById('DeleteAlbumLoading').style.display = 'block';
    var xmlhttprequest = getXmlHttpRequest();
    xmlhttprequest.open("POST", "Ashx/ValiEmail.ashx", true);
    xmlhttprequest.onreadystatechange = function () {
        if (xmlhttprequest.readyState == 4 && xmlhttprequest.status == 200) {
            document.getElementById('DeleteAlbumLoading').style.display = 'none';
            if (xmlhttprequest.responseText == '1') {
                document.getElementById('valiEmailText').innerHTML = '邮件已发送,请进入邮箱确认.';
            }
            else if (xmlhttprequest.responseText == '-1') {
                document.getElementById('valiEmailText').innerHTML = '未登录无法发送邮件.';
            }
            else {
                document.getElementById('valiEmailText').innerHTML = '邮箱已经验证.';
            }
            xmlhttprequest = null;
        }
        else if (xmlhttprequest.status == 500) {
            xmlhttprequest = null;
            document.getElementById('DeleteAlbumLoading').style.display = 'none';
            document.getElementById('valiEmailText').innerHTML = '服务器错误.';
        }
    }
    xmlhttprequest.send();

}

function getXmlNodeValue(xmlNode) {
    var value = '';
    try {
        value = xmlNode.childNodes[0].nodeValue;
    }
    catch (e) {
        value = '';
    }
    return value;
}