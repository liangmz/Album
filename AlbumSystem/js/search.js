﻿/*获得异步请求*/
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

/*读取URL中get参数*/
function request(strParame) {
    var args = new Object();
    var query = location.search.substring(1);

    var pairs = query.split("&"); // Break at ampersand 
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');
        if (pos == -1) continue;
        var argname = pairs[i].substring(0, pos);
        var value = pairs[i].substring(pos + 1);
        value = decodeURIComponent(value);
        args[argname] = value;
    }
    return args[strParame];
}

/*回调函数*/
function setHtml(responseText)
{
        var xml = responseText;
        //alert(xml);
        var xmlDom = getXML(xml);
        var root = xmlDom.documentElement;
        sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('SearchPageCount')[0]);
        var searchList = root.getElementsByTagName('Search')[0];
        //alert(pageCount);
        LA.innerHTML = '';
        LB.innerHTML = '';
        LC.innerHTML = '';
        var E = function (j) {
            switch (j) {
                case 0: { return document.getElementById('LA'); };
                case 1: { return document.getElementById('LB'); };
                case 2: { return document.getElementById('LC'); };
            }
        };
        //建立结果页面
        for (var i = 0,j=0 ; i < searchList.childNodes.length ;i++,j=(j+1)%3)
        {
            var user = searchList.childNodes[i];
            var UID = getXmlNodeValue(user.getElementsByTagName('UID')[0]);
            var USERNAME = getXmlNodeValue(user.getElementsByTagName('USERNAME')[0]);
            var EMAIL = getXmlNodeValue(user.getElementsByTagName('EMAIL')[0]);
            var EPIC = getXmlNodeValue(user.getElementsByTagName('PIC')[0]);
            var MIN = E(j);
            MIN.appendChild(getUserE(UID,USERNAME,EMAIL,EPIC));
        }
        //建立分页栏
        //page pageCount 全局变量Javascript.js
        buildPage(sessionStorage.pageCount, sessionStorage.page, 'return changePage(this)');
}



/*////////////////////////////执行/////////////////////////////*/

window.onload = function () {
    setLoadingNode();
    var search = request('keyword');
    if (search.length > 0) {
        var data = new FormData();
        data.append("page", sessionStorage.page);
        data.append("keyword", search);
        var httpxmlrequest = getXmlHttpRequest();

        httpxmlrequest.open("post", "Ashx/Search.ashx", true);
        httpxmlrequest.onreadystatechange = function(){
            if (httpxmlrequest.readyState == 4 && httpxmlrequest.status == 200) {
                setHtml(httpxmlrequest.responseText);
            }
        }
        httpxmlrequest.send(data);
    }

    //window['TLookUID'] = -1;
    window.TLookUID = -1;
    $('#Look').on('hidden.bs.modal', function (e) {
        TLookUID = -1;
    });
}
/*////////////////////////////执行//////////////////////////////*/

/*获得xml解析 JavaScript.js存在改函数 
function getXML(xml)
{
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
*/

/*获得高度最小的DIV*/
function getMin()
{
    var MIN = LA.offsetHeight <= LB.offsetHeight ? LA : LB;
    MIN = MIN.offsetHeight <= LC.offsetHeight ? MIN : LC;
    return MIN;
}

/*获取一个用户节点*/
function getUserE(UID,userName,email,src)
{
    var div = document.createElement('div');
    div.className = 'media';

    var div_a = document.createElement('a');
    div_a.className = 'pull-left';
    var div_a_img = document.createElement('img');
    div_a_img.src = src;//头像路径
    div_a_img.className = 'media-object';
    div_a_img.style.width = '64px';
    div_a_img.style.height = '64px';
    div_a.appendChild(div_a_img);

    div.appendChild(div_a);

    var div_div = document.createElement('div');
    div_div.className = 'media-body';
    var div_div_btn = document.createElement('button');
    div_div_btn.type = 'button';
    div_div_btn.className = 'btn btn-primary btn-sm pull-right';
    div_div_btn.innerHTML = '关注';
    div_div_btn.setAttribute('onclick', 'look(this);');
    div_div_btn.setAttribute('UID',UID);//UID

    div_div.appendChild(div_div_btn);

    var div_div_h3 = document.createElement('h3');
    div_div_h3.className = 'media-heading';
    var div_div_h3_a = document.createElement('a');
    div_div_h3_a.innerHTML = userName;//用户名
    div_div_h3_a.href = "mailto:#";
    div_div_h3_a.setAttribute('onclick','return false;');

    div_div_h3.appendChild(div_div_h3_a);
    div_div.appendChild(div_div_h3);

    var div_div_a = document.createElement('a');
    div_div_a.href = 'mailto:#';
    div_div_a.innerHTML = email;//Email
    div_div_a.setAttribute('onclick', 'return false;');

    div_div.appendChild(div_div_a);
    div.appendChild(div_div);

    return div;
}


/*换页面执行*/
function changePageActive(goPage)
{
    //setLoadingNode();
    console.warn(goPage);
    var search = request('keyword');
    if (search.length > 0) {
        var data = new FormData();
        data.append("page", goPage);
        data.append("keyword", search);
        var xmlrequest = getXmlHttpRequest();

        xmlrequest.open("post", "Ashx/Search.ashx", true);
        xmlrequest.onreadystatechange = function () {
            if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
                var xml = xmlrequest.responseText;
                var xmlDom = getXML(xml);
                //alert(xml);
                var root = xmlDom.documentElement;
                sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('SearchPageCount')[0]);
                var searchList = root.getElementsByTagName('Search')[0];
                //alert(pageCount);
                LA.innerHTML = '';
                LB.innerHTML = '';
                LC.innerHTML = '';
                var E = function (j) {
                    switch (j) {
                        case 0: { return document.getElementById('LA'); };
                        case 1: { return document.getElementById('LB'); };
                        case 2: { return document.getElementById('LC'); };
                    }
                };
                //建立结果页面
                for (var i = 0, j = 0 ; i < searchList.childNodes.length ; i++, j = (j + 1) % 3) {
                    var user = searchList.childNodes[i];
                    var UID = getXmlNodeValue(user.getElementsByTagName('UID')[0]);
                    var USERNAME = getXmlNodeValue(user.getElementsByTagName('USERNAME')[0]);
                    var EMAIL = getXmlNodeValue(user.getElementsByTagName('EMAIL')[0]);
                    var EPIC = getXmlNodeValue(user.getElementsByTagName('PIC')[0]);
                    var MIN = E(j);
                    MIN.appendChild(getUserE(UID, USERNAME, EMAIL,EPIC));
                }
            }
        }
        xmlrequest.send(data);
    }
}


/*按下关注*/
function look(e) {
    TLookUID = e.getAttribute('UID');
    $('#Look').modal('show');

}
/*确定关注这位哥*/
function lookActive() {
    if (TLookUID > 0) {
        var httprequest = getXmlHttpRequest();
        httprequest.open('post', 'Ashx/Look.ashx', true);
        httprequest.onreadystatechange = function () {
            if (httprequest.readyState == 4 && httprequest.status == 200) {
                if (httprequest.responseText == '-1') {
                    $('#Lose').modal('show');
                    $('#Look').modal('hide');
                }
                else {
                    $('#Success').modal('show');
                    $('#Look').modal('hide');
                }
            }
        }
        var data = new FormData();
        data.append('UID', TLookUID);
        httprequest.send(data);
    }
}


function setLoadingNode() {
    LA.appendChild(loadingNode('LoadA'));
    LB.appendChild(loadingNode('LoadB'));
    LC.appendChild(loadingNode('LoadC'));
}
function loadingNode(id) {
    var L = document.createElement('div');
    L.setAttribute('id', id);
    L.className = 'thumbnail';
    L.innerHTML = '<span class="glyphicon glyphicon-refresh tagLoading"></span>  Loading...';
    return L;
}