window.onload = function () {
    setLoadingNode();

    var searchGo = document.getElementById('searchGo');
    var actionText = document.getElementById('actionText');

    searchGo.setAttribute('value', 1);
    actionText.innerHTML = '图片<span class="caret"></span>';

    changePageActive(1);
    
}


function changePageActive(_page) {
    sessionStorage.page = _page;
    //console.warn(_page);
    var search = request('keyword');
    if (search.length > 0) {

        var data = new FormData();
        data.append('keyword', search);
        data.append('page', _page);

        var xmlrequest = getXmlHttpRequest();
        xmlrequest.open("post", "Ashx/SearchPic.ashx", true);
        xmlrequest.onreadystatechange = function () {
            if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
                var xml = xmlrequest.responseText;
                //alert(xml);
                var xmlDom = getXML(xml);
                var root = xmlDom.documentElement;
                sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('SearchPicPageCount')[0]) ;
                var Share = root.getElementsByTagName('SearchPic')[0];

                PA.innerHTML = '';
                PB.innerHTML = '';
                PC.innerHTML = '';
                PE.innerHTML = '';
                PD.innerHTML = '';
                PF.innerHTML = '';

                var E = function () {
                    var Min = PA.offsetHeight <= PB.offsetHeight ? PA : PB;
                    Min = Min.offsetHeight <= PC.offsetHeight ? Min : PC;
                    Min = Min.offsetHeight <= PD.offsetHeight ? Min : PD;
                    Min = Min.offsetHeight <= PE.offsetHeight ? Min : PE;
                    Min = Min.offsetHeight <= PF.offsetHeight ? Min : PF;
                    return Min;

                };
                //建立结果页面
                for (var i = 0 ; i < Share.childNodes.length ; i++) {
                    var user = Share.childNodes[i];
                    var PID = getXmlNodeValue(user.getElementsByTagName('PID')[0]);
                    var PNAME = getXmlNodeValue(user.getElementsByTagName('PNAME')[0]);
                    var PTIME =getXmlNodeValue( user.getElementsByTagName('PTIME')[0]);
                    var PDES =getXmlNodeValue( user.getElementsByTagName('PDES')[0]);
                    var PDATA = getXmlNodeValue(user.getElementsByTagName('PDATA')[0]);
                    var MIN = E();
                    MIN.appendChild(getPic(PID, PNAME, PTIME, PDES, PDATA));
                }
                buildPage(sessionStorage.pageCount, sessionStorage.page, 'return changePage(this)');
            }
        }
        xmlrequest.send(data);
    }
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

function setLoadingNode() {
    PA.appendChild(loadingNode('LA'));
    PB.appendChild(loadingNode('LB'));
    PC.appendChild(loadingNode('LC'));
    PD.appendChild(loadingNode('LD'));
    PE.appendChild(loadingNode('LE'));
    PF.appendChild(loadingNode('LF'));
}
function loadingNode(id) {
    var L = document.createElement('div');
    L.setAttribute('id', id);
    L.className = 'thumbnail';
    L.innerHTML = '<span class="glyphicon glyphicon-refresh tagLoading"></span>  Loading...';
    return L;
}