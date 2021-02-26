//  https://tmnforever.tm-exchange.com/main.aspx?action=tracksearch&mode=1&id=1001

var tr1 = document.getElementsByClassName("WindowTableCell1");
var tr2 = document.getElementsByClassName("WindowTableCell2");

var trAll = [];

var json = {"Tracks": []};

for (var i = 0; i < tr1.length; i++) {
    trAll.push(tr1[i]);
}

for (var i = 0; i < tr2.length; i++) {
    trAll.push(tr2[i]);
}

for (var i = 0; i < trAll.length; i++) {
    var tds = trAll[i].getElementsByTagName("td");
    var trackName = tds[0].getElementsByTagName("a")[2].innerText;
    var trackId = tds[0].getElementsByTagName("a")[2].href;
    trackId = trackId.split("id=")[1];
    trackId = trackId.split("#")[0];
    var trackAuthor = tds[1].innerText.substring(2);
    var wrTime = tds[6].innerText;
    var wrAuthor = tds[7].getElementsByTagName("a")[2].innerText;
    json["Tracks"].push({
        TrackName: trackName,
        TrackAuthor: trackAuthor,
        WRTime: wrTime,
        WRAuthor: wrAuthor,
        TrackId: trackId
    })
}

console.log(JSON.stringify(json));