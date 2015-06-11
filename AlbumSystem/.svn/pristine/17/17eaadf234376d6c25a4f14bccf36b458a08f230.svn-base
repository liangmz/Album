window.onload = function () {
    
    changePageActive(1);
    //buildPage(sessionStorage.pageCount, 1, 'return changePage(this)');
}

function changePageActive(_page) {
    setLoadingNode();
    sessionStorage.page = _page;

    console.warn(_page);
    var data = new FormData();
    data.append('page', _page);

    var xmlrequest = getXmlHttpRequest();
    xmlrequest.open("post", "Ashx/ShareU.ashx", true);
    xmlrequest.onreadystatechange = function () {
        if (xmlrequest.readyState == 4 && xmlrequest.status == 200) {
            var xml = xmlrequest.responseText;
            //alert(xml);
            var xmlDom = getXML(xml);
            var root = xmlDom.documentElement;
            sessionStorage.pageCount = getXmlNodeValue(root.getElementsByTagName('UserSharePageCount')[0]) ;
            var Share = root.getElementsByTagName('UserShare')[0];

            var E = function () {
                var Min = PA.offsetHeight <= PB.offsetHeight ? PA : PB;
                Min = Min.offsetHeight <= PC.offsetHeight ? Min : PC;
                Min = Min.offsetHeight <= PD.offsetHeight ? Min : PD;
                Min = Min.offsetHeight <= PE.offsetHeight ? Min : PE;
                Min = Min.offsetHeight <= PF.offsetHeight ? Min : PF;
                return Min;

            };
            PA.innerHTML = '';
            PB.innerHTML = '';
            PC.innerHTML = '';
            PD.innerHTML = '';
            PE.innerHTML = '';
            PF.innerHTML = '';

            //建立结果页面
            for (var i = 0 ; i < Share.childNodes.length ; i++) {
                var user = Share.childNodes[i];
                var PID = getXmlNodeValue(user.getElementsByTagName('PID')[0]);
                var PNAME = getXmlNodeValue(user.getElementsByTagName('PNAME')[0]);
                var PTIME =getXmlNodeValue( user.getElementsByTagName('PTIME')[0]);
                var PDES = getXmlNodeValue(user.getElementsByTagName('PDES')[0]);
                var PDATA = getXmlNodeValue(user.getElementsByTagName('PDATA')[0]);
                var MIN = E();
                MIN.appendChild(getPic(PID, PNAME, PTIME, PDES, PDATA));
            }

            buildPage(sessionStorage.pageCount, sessionStorage.page, 'return changePage(this)');
        }
    }
    xmlrequest.send(data);
}



//function removeLoadingNode() {
//    LA.remove();
//    LB.remove();
//    LC.remove();
//    LD.remove();
//    LE.remove();
//    LF.remove();
//}
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