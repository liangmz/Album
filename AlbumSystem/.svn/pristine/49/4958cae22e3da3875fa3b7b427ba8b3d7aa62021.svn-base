
function getPic(pid, title, time, des, src) {
    var div = document.createElement('div');
    div.className = 'thumbnail';

    var picContent = '<a href="ThisPic.aspx?pid=' + pid + '">' +
            '<img src="" lazy_src="' + src + '" class="img-responsive img-rounded mypic" style="max-height:200px;">' +
        '</a>' +
        '<div class="caption">' +
            '<h6 style="word-break:break-all;">' + title + '</h6>' +
            '<a href="javascript:void(0)">' + time + '</a>' +
        '</div>';
    div.innerHTML = picContent;
    return div;
}

function LazyInit() {
    sessionStorage.lazy_Min = [0, 0, 0, 0, 0, 0];//Div node index
    sessionStorage.lazy_Div = ['PA', 'PB', 'PC', 'PD', 'PE', 'PF'];
    sessionStorage.Min_Height = 0;
    sessionStorage.lazyDebugMode = 0;
}

function Load(Node) {
    try {
        Node.setAttribute('src', Node.getAttribute('lazy_src'));
        Node.removeAttribute('lazy_src');
        if (sessionStorage.lazyDebugMode == '1') {
            console.log(Node.getAttribute('src'));
        }
    } catch (E) { }
}

function LazyLoad(height) {
    if (Number(height) < Number(sessionStorage.Min_Height))
        return;
    if (sessionStorage.lazyDebugMode == '1') {
        console.log('lazyLoad...' + height);
    }
    var FLAG = new Array();// [true, true, true, true, true, true];
    var Ans = true;
    var updateMinHeight = false;
    var DivList = sessionStorage.lazy_Div.split(',');
    var lazyMin = sessionStorage.lazy_Min.split(',');
    for (var f = 0; f < DivList.length; f++) {
        FLAG[f] = true;
    }
    while (Ans) {
        for (var i = 0 ; i < DivList.length; i++) {
            if (!FLAG[i]) continue;
            try {
                var thumbnail = document.getElementById(DivList[i]).children[Number(lazyMin[i])];
                var imgNode = getImgNode(thumbnail);
                if (imgNode != null && Number(imgNode.offsetTop) < Number(height)) {
                    if (!updateMinHeight)
                    {
                        sessionStorage.Min_Height = Number(imgNode.offsetTop);
                        updateMinHeight = true;
                    }
                    else if (Number(imgNode.offsetTop) < sessionStorage.Min_Height)
                    {
                        sessionStorage.Min_Height = Number(imgNode.offsetTop);
                    }
                    Load(imgNode);
                    lazyMin[i] = Number(lazyMin[i]) + Number(1);
                } else {
                    FLAG[i] = false;
                }
            } catch (Ex) {
                FLAG[i] = false;
            }
        }
        Ans = FLAG[0];
        for (var k = 1 ; k < FLAG.length ; k++) {
            Ans = Ans || FLAG[k];
        }
    }
    sessionStorage.lazy_Min = lazyMin[0];
    for (var i = 1; i < lazyMin.length ; i++) {
        sessionStorage.lazy_Min += ',' + lazyMin[i];
    }
    if (sessionStorage.lazyDebugMode == '1') {
        console.log('lazyLoad...End');
    }
}

function getImgNode(divNode) {
    try {
        return divNode.getElementsByTagName('img')[0];
    } catch (ex) {
        return null;
    }
}