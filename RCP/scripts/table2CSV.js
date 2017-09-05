jQuery.fn.table2CSV = function(section) {        // header, report, footer ...
    var csvData = [];
    var headerArr = [];
    var el = this;
    var el0 = el.get(0);
    if (el0 != null) {
        var tag = el.get(0).tagName;
        if (tag == "TABLE") {
            //header
            var tmpRow = []; // construct header avalible array
            $(el).filter(':visible').find('th').each(function() {
                if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
            });
            row2CSV(tmpRow);
            // actual data
            $(el).find('tr').each(function() {
                var tmpRow = [];
                $(this).filter(':visible').find('td').each(function() {
                    if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
                });
                row2CSV(tmpRow);
            });
        }
        else if (tag == "DIV") {
            $(el).find('span').each(function() {
                if ($(this).css('display') != 'none') {
                    var tmpRow = [];
                    tmpRow[tmpRow.length] = formatData($(this).html());
                    row2CSV(tmpRow);
                }
            });
        }
    }
    var mydata;
    if (csvData.length > 0)
        mydata = '[' + section + ']\n' + csvData.join('\n') + '\n';
    //mydata = '[' + section + ']\r' + csvData.join('\r') + '\r';  // nie robi, po stronie serwera zawsze jest cr lf na końcu
    else
        mydata = '';


    //alert(mydata);


    return mydata;

    //---------------------------
    function row2CSV(tmpRow) {
        var tmp = tmpRow.join('') // to remove any blank rows
        if (tmpRow.length > 0 && tmp != '') {
            var mystr = tmpRow.join('\t');
            csvData[csvData.length] = mystr;
        }
    }

    function formatData(input) {
        // replace " with “

        /*
        var regexp = new RegExp(/["]/g);
        var output = input.replace(regexp, "“");
        */

        // cr lf i spacje z początku następnej linii ! uwaga input a nie output.replace
        var regexp = new RegExp(/[\n] */g);
        var output = input.replace(regexp, "");

        //alert(input + "\n---------------------------\n" + output);

        // <br> -> cr lf
        regexp = new RegExp(/\<[Bb][Rr]\>/g);
        output = jQuery.trim(output.replace(regexp, "\n"));
        //output = jQuery.trim(output.replace(regexp, "\r"));

        //var o = output;

        //HTML -> ""
        regexp = new RegExp(/\<[^\<]+\>/g);
        output = jQuery.trim(output.replace(regexp, ""));

        //alert(o + "\n---------------------------\n" + output);

        if (output == "") return '';
        //return '"' + output + '"';
        return output;
    }
};




/*
jQuery.fn.table2CSV = function(options) {
    var options = jQuery.extend({
        separator: ',',
        header: [],
        delivery: 'popup' // popup, value
    },
    options);

    var csvData = [];
    var headerArr = [];
    var el = this;

    //header
    var numCols = options.header.length;
    var tmpRow = []; // construct header avalible array

    if (numCols > 0) {
        for (var i = 0; i < numCols; i++) {
            tmpRow[tmpRow.length] = formatData(options.header[i]);
        }
    } else {
        $(el).filter(':visible').find('th').each(function() {
            if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
        });
    }

    row2CSV(tmpRow);

    // actual data
    $(el).find('tr').each(function() {
        var tmpRow = [];
        $(this).filter(':visible').find('td').each(function() {
            if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
        });
        row2CSV(tmpRow);
    });
    if (options.delivery == 'popup') {
        var mydata = csvData.join('\n');
        return popup(mydata);
    } else {
        var mydata = csvData.join('\n');
        return mydata;
    }

    function row2CSV(tmpRow) {
        var tmp = tmpRow.join('') // to remove any blank rows
        // alert(tmp);
        if (tmpRow.length > 0 && tmp != '') {
            var mystr = tmpRow.join(options.separator);
            csvData[csvData.length] = mystr;
        }
    }
    function formatData(input) {
        // replace " with “
        var regexp = new RegExp(/["]/g);
        var output = input.replace(regexp, "“");
        //HTML
        var regexp = new RegExp(/\<[^\<]+\>/g);
        var output = output.replace(regexp, "");
        if (output == "") return '';
        return '"' + output + '"';
    }
    function popup1(data) {
        var generator = window.open('', 'csv', 'height=400,width=600');
        generator.document.write('<html><head><title>CSV</title>');
        generator.document.write('</head><body >');
        generator.document.write('<textArea cols=70 rows=15 wrap="off" >');
        generator.document.write(data);
        generator.document.write('</textArea>');
        generator.document.write('</body></html>');
        generator.document.close();
        return true;
    }

    function popup(data) {
        //var generator = window.open('', 'csv', 'height=400,width=600');
        var generator = window.open('', 'csv', '');
        generator.document.write('<html><head><title>CSV</title>');

        //generator.document.write('<meta http-equiv="Content-Type" content="application/vnd.ms-excel"; charset=utf8>');
        generator.document.write('<meta http-equiv="Content-Type" content="application/vnd.ms-excel">');
        generator.document.write('<meta http-equiv="Content-Disposition" content="attachment; filename=asdf.xls">');
        //generator.document.write('<meta http-equiv="refresh" content="2">');
        
        generator.document.write('</head><body >');
        //generator.document.write(data);
        generator.document.write('</body></html>');
        generator.document.close();
        return true;
    }

    /*    
    
    Response.Clear();
    Response.Buffer = true;
    Response.ContentType = "application/vnd.ms-excel";
    Response.ContentEncoding = Encoding.Unicode;
    Response.Charset = "unicode";
    this.EnableViewState = false;
    Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".csv");
    StringWriter stringWriter = new StringWriter();

            string d;
    string line = null;
    int cnt = ds.Tables[0].Columns.Count;
    for (int i = 0; i < cnt; i++)
    {
    d = CtrlToText(ds.Tables[0].Columns[i].ToString());
    if (i == 0)
    line = d;
    else
    line += TAB + d;
    }
    stringWriter.WriteLine(line);

            foreach (DataRow dr in ds.Tables[0].Rows)
    {
    for (int i = 0; i < cnt; i++)
    {
    d = CtrlToText(dr[i].ToString());
    if (i == 0)
    line = d;
    else
    line += TAB + d;
    }
    stringWriter.WriteLine(line);
    }
    Response.Write((char)65279);  // FF FE - UTF-16 Little Endian BOM , bez tego excel nie chce poprawnie pliku odczytywać
    Response.Write(stringWriter.ToString());
                
    * /


};
*/