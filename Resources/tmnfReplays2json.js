var downloadBase = "https://tmnforever.tm-exchange.com/get.aspx?action=recordgbx&id=";

var trackName = document.getElementById("ctl03_ShowTrackName").innerText;
var trackId = document.getElementById("ctl03_DoDownload").href.split("id=")[1];
console.log(trackId);

var tr1 = document.getElementsByClassName("WindowTableCell1");
var tr2 = document.getElementsByClassName("WindowTableCell2");

var trAll = [];

for (var i = 0; i < tr1.length; i++) {
    trAll.push(tr1[i]);
}

for (var i = 0; i < tr2.length; i++) {
    trAll.push(tr2[i]);
}

console.log(trAll);

var curlHeader = "xd u thought";
var curlQueries = "";

trAll.forEach(el => {
    var tds = el.getElementsByTagName("td");
    if (tds.length == 5) {
        var replayAuthor = tds[1].innerText.substring(2).replace("'", "");
        var replayTime = tds[0].innerText.substring(1);
        console.log(replayAuthor);
        var replayUrl = tds[0].getElementsByTagName("a")[0].href;
        console.log(replayUrl);
        curlQueries += "mkdir '" + trackName + "_" + trackId + "' ;curl '" + replayUrl + "' " + curlHeader + " > '" + trackName + "_" + trackId + "/" + replayAuthor + "_" + replayTime + ".gbx' ;" ;
    }
});

console.log(curlQueries);
