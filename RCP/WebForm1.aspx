<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="HRRcp.WebForm1" %>




<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
<title>Untitled Document</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="Content-Style-Type" content="text/css"/>
<meta http-equiv="Content-Script-Type" content="text/javascript"/>


  <script src="http://code.jquery.com/jquery-1.9.1.js" type="text/jscript"></script>






</head>
<body>

<div id="Iframe2" style="width: 600px; height: 600px; border: solid 1px red;">
</div>

<script type="text/javascript">
    //$('#Iframe2').load('https://sites.google.com/a/jabil.com/bydgoszcz-intranet/');
    //$('#Iframe2').load('http://www.jarwal.com.pl');
    $('#Iframe2').load('http://www.kdrsolutions.pl');
    //$('#Iframe2').load('Portal.aspx');
</script>

-1-----------------------------<br />
<iframe src="https://sites.google.com/a/jabil.com/bydgoszcz-intranet/"></iframe>
<iframe id="Iframe1" src="https://sites.google.com/a/jabil.com/bydgoszcz-intranet/" frameborder="0" width="100%" height="700"></iframe>
-2-----------------------------<br />
<div id="wrap"><object type="text/html" data="https://sites.google.com/a/jabil.com/bydgoszcz-intranet/"></object></div>
-3-----------------------------<br />

<iframe id="forum_embed"
  src="javascript:void(0)"
  scrolling="no"
  frameborder="0"
  width="900"
  height="700">
</iframe>
<script type="text/javascript">
    document.getElementById('forum_embed').src =
     'https://sites.google.com/a/jabil.com/bydgoszcz-intranet/';
     /*
     + '&showsearch=true&showpopout=true&showtabs=false'
     + '&parenturl=' + encodeURIComponent(window.location.href);
     */
</script>

-4-----------------------------<br />

<div id="Iframe2a">
</div>
<script type="text/javascript">
    //$('#Iframe2a').load('https://sites.google.com/a/jabil.com/bydgoszcz-intranet/');
    $('#Iframe2a').load('http://www.jarwal.com.pl');
</script>

-5-----------------------------<br />

</body>
</html>


