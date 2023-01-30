function createPDF(title) {
    var sTable = $('#print').html()
    var style = "<style>";
    style = style + "table {width: 100%;font: 17px Calibri;}";
    style = style + "table, th, td {border: solid 1px #DDD; border-collapse: collapse;";
    style = style + "padding: 2px 3px;text-align: center;}";
    style = style + ".createPdfBlock {display: none;} dt { font-weight: 600; } dd { border-bottom: 1px solid #999; font-style: italic; margin: 0 0 10px 0; }";
    style = style + "</style>";

    var win = window.open('', '', 'height=700,width=700');
    win.document.write('<html><head>');
    win.document.write(`<title>${title}</title>`);
    win.document.write(style);
    win.document.write('</head>');
    win.document.write('<body>');
    win.document.write(`<h1 style='text-align: center;'>${title}</h1>`);
    win.document.write(sTable);
    win.document.write('</body></html>');
    win.document.close();
    win.print();
}