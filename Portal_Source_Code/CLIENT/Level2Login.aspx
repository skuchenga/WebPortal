<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Level2Login.aspx.cs" Inherits="Level2Login" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript" src="vkboard.js"></script>

    <script><!--
        window.history.forward(0);
        // This example shows how to handle multiple INPUT
        // fields with a single vkeyboard.

        // Parts of the following code are taken from the DocumentSelection
        // library (http://debugger.ru/projects/browserextensions/documentselection)
        // by Ilya Lebedev. DocumentSelection is distributed under LGPL license
        // (http://www.gnu.org/licenses/lgpl.html).

        // 'source' is the field which is currently focused:
        var source = null, vkb = null, opened = true, insertionS = 0, insertionE = 0;

        var userstr = navigator.userAgent.toLowerCase();
        var safari = (userstr.indexOf('applewebkit') != -1);
        var gecko = (userstr.indexOf('gecko') != -1) && !safari;
        var standr = gecko || window.opera || safari;

        // This function retrieves the source element
        // for the given event object:
        function get_event_source(e) {
            var event = e || window.event;
            return event.srcElement || event.target;
        }

        // This function binds 'handler' function to the 
        // 'eventType' event of the 'elem' element:
        function setup_event(elem, eventType, handler) {
            return (elem.attachEvent) ? elem.attachEvent("on" + eventType, handler) : ((elem.addEventListener) ? elem.addEventListener(eventType, handler, false) : false);
        }

        // By focusing the INPUT field we set the 'source'
        // to the newly focused field:
        function focus_keyboard(e) {
            source = get_event_source(e);
        }

        // By "registering" the field we bind 'focus_keyboard'
        // function to 'focus' event of the given INPUT field:
        function register_field(id) {
            setup_event(document.getElementById(id), "focus", focus_keyboard);
        }

        // The same creation precedure as usual, with a single
        // difference that we're "registering" 3 INPUT fields:
        function init() {
            // Note: all parameters, starting with 3rd, in the following
            // expression are equal to the default parameters for the
            // VKeyboard object. The only exception is 18th parameter
            // (flash switch), which is false by default.

            vkb = new VKeyboard("keyboard",    // container's id
					       keyb_callback, // reference to the callback function
                           true,          // create the arrow keys or not? (this and the following params are optional)
                           true,          // create up and down arrow keys? 
                           false,         // reserved
                           true,          // create the numpad or not?
                           "",            // font name ("" == system default)
					       "14px",        // font size in px
                           "#000",        // font color
                           "#F00",        // font color for the dead keys
                           "#FFF",        // keyboard base background color
                           "#FFF",        // keys' background color
                           "#DDD",        // background color of switched/selected item
                           "#777",        // border color
                           "#CCC",        // border/font color of "inactive" key (key with no value/disabled)
                           "#FFF",        // background color of "inactive" key (key with no value/disabled)
                           "#F77",        // border color of the language selector's cell
                           true,          // show key flash on click? (false by default)
                           "#CC3300",     // font color during flash
                           "#FF9966",     // key background color during flash
                           "#CC3300",     // key border color during flash
                           false,         // embed VKeyboard into the page?
                           true,          // use 1-pixel gap between the keys?
                           0);            // index(0-based) of the initial layout

            // 'field1' is "focused" by default:
            source = document.getElementById("txtPassword");

            register_field("txtPassword"); //register_field("field2"); register_field("field3");

            source.focus();
        }

        function keyb_change() {
            opened = !opened;
            vkb.Show(opened);
        }

        // Advanced callback function:
        //
        function keyb_callback(ch) {
            var val = source.value;

            switch (ch) {
                case "BackSpace":
                    if (val.length) {
                        var span = null;

                        if (document.selection)
                            span = document.selection.createRange().duplicate();

                        if (span && span.text.length > 0) {
                            span.text = "";
                            getCaretPositions(source);
                        }
                        else
                            deleteAtCaret(source);
                    }

                    break;

                case "<":
                    if (insertionS > 0)
                        setRange(source, insertionS - 1, insertionE - 1);

                    break;

                case ">":
                    if (insertionE < val.length)
                        setRange(source, insertionS + 1, insertionE + 1);

                    break;

                case "/\\":
                    if (!standr) break;

                    var prev = val.lastIndexOf("\n", insertionS) + 1;
                    var pprev = val.lastIndexOf("\n", prev - 2);
                    var next = val.indexOf("\n", insertionS);

                    if (next == -1) next = val.length;
                    var nnext = next - insertionS;

                    if (prev > next) {
                        prev = val.lastIndexOf("\n", insertionS - 1) + 1;
                        pprev = val.lastIndexOf("\n", prev - 2);
                    }

                    // number of chars in current line to the left of the caret:
                    var left = insertionS - prev;

                    // length of the prev. line:
                    var plen = prev - pprev - 1;

                    // number of chars in the prev. line to the right of the caret:
                    var right = (plen <= left) ? 1 : (plen - left);

                    var change = left + right;
                    setRange(source, insertionS - change, insertionE - change);

                    break;

                case "\\/":
                    if (!standr) break;

                    var prev = val.lastIndexOf("\n", insertionS) + 1;
                    var next = val.indexOf("\n", insertionS);
                    var pnext = val.indexOf("\n", next + 1);

                    if (next == -1) next = val.length;
                    if (pnext == -1) pnext = val.length;

                    if (pnext < next) pnext = next;

                    if (prev > next)
                        prev = val.lastIndexOf("\n", insertionS - 1) + 1;

                    // number of chars in current line to the left of the caret:
                    var left = insertionS - prev;

                    // length of the next line:
                    var nlen = pnext - next;

                    // number of chars in the next line to the left of the caret:
                    var right = (nlen <= left) ? 0 : (nlen - left - 1);

                    var change = (next - insertionS) + nlen - right;
                    setRange(source, insertionS + change, insertionE + change);

                    break;

                default:
                    insertAtCaret(source, (ch == "Enter" ? (window.opera ? '\r\n' : '\n') : ch));
            }
        }

        // This function retrieves the position (in chars, relative to
        // the start of the text) of the edit cursor (caret), or, if
        // text is selected in the TEXTAREA, the start and end positions
        // of the selection.
        //
        function getCaretPositions(ctrl) {
            var CaretPosS = -1, CaretPosE = 0;

            // Mozilla way:
            if (ctrl.selectionStart || (ctrl.selectionStart == '0')) {
                CaretPosS = ctrl.selectionStart;
                CaretPosE = ctrl.selectionEnd;

                insertionS = CaretPosS == -1 ? CaretPosE : CaretPosS;
                insertionE = CaretPosE;
            }
            // IE way:
            else if (document.selection && ctrl.createTextRange) {
                var start = end = 0;
                try {
                    start = Math.abs(document.selection.createRange().moveStart("character", -10000000)); // start

                    if (start > 0) {
                        try {
                            var endReal = Math.abs(ctrl.createTextRange().moveEnd("character", -10000000));

                            var r = document.body.createTextRange();
                            r.moveToElementText(ctrl);
                            var sTest = Math.abs(r.moveStart("character", -10000000));
                            var eTest = Math.abs(r.moveEnd("character", -10000000));

                            if ((ctrl.tagName.toLowerCase() != 'input') && (eTest - endReal == sTest))
                                start -= sTest;
                        }
                        catch (err) { }
                    }
                }
                catch (e) { }

                try {
                    end = Math.abs(document.selection.createRange().moveEnd("character", -10000000)); // end
                    if (end > 0) {
                        try {
                            var endReal = Math.abs(ctrl.createTextRange().moveEnd("character", -10000000));

                            var r = document.body.createTextRange();
                            r.moveToElementText(ctrl);
                            var sTest = Math.abs(r.moveStart("character", -10000000));
                            var eTest = Math.abs(r.moveEnd("character", -10000000));

                            if ((ctrl.tagName.toLowerCase() != 'input') && (eTest - endReal == sTest))
                                end -= sTest;
                        }
                        catch (err) { }
                    }
                }
                catch (e) { }

                insertionS = start;
                insertionE = end
            }
        }

        function setRange(ctrl, start, end) {
            if (ctrl.setSelectionRange) // Standard way (Mozilla, Opera, Safari ...)
            {
                ctrl.setSelectionRange(start, end);
            }
            else // MS IE
            {
                var range;

                try {
                    range = ctrl.createTextRange();
                }
                catch (e) {
                    try {
                        range = document.body.createTextRange();
                        range.moveToElementText(ctrl);
                    }
                    catch (e) {
                        range = null;
                    }
                }

                if (!range) return;

                range.collapse(true);
                range.moveStart("character", start);
                range.moveEnd("character", end - start);
                range.select();
            }

            insertionS = start;
            insertionE = end;
        }

        function deleteSelection(ctrl) {
            if (insertionS == insertionE) return;

            var tmp = (document.selection && !window.opera) ? ctrl.value.replace(/\r/g, "") : ctrl.value;
            ctrl.value = tmp.substring(0, insertionS) + tmp.substring(insertionE, tmp.length);

            setRange(ctrl, insertionS, insertionS);
        }

        function deleteAtCaret(ctrl) {
            // if(insertionE < insertionS) insertionE = insertionS;
            if (insertionS != insertionE) {
                deleteSelection(ctrl);
                return;
            }

            if (insertionS == insertionE)
                insertionS = insertionS - 1;

            var tmp = (document.selection && !window.opera) ? ctrl.value.replace(/\r/g, "") : ctrl.value;
            ctrl.value = tmp.substring(0, insertionS) + tmp.substring(insertionE, tmp.length);

            setRange(ctrl, insertionS, insertionS);
        }

        // This function inserts text at the caret position:
        //
        function insertAtCaret(ctrl, val) {
            if (insertionS != insertionE) deleteSelection(ctrl);

            if (gecko && document.createEvent && !window.opera) {
                var e = document.createEvent("KeyboardEvent");

                if (e.initKeyEvent && ctrl.dispatchEvent) {
                    e.initKeyEvent("keypress",        // in DOMString typeArg,
                        false,             // in boolean canBubbleArg,
                        true,              // in boolean cancelableArg,
                        null,              // in nsIDOMAbstractView viewArg, specifies UIEvent.view. This value may be null;
                        false,             // in boolean ctrlKeyArg,
                        false,             // in boolean altKeyArg,
                        false,             // in boolean shiftKeyArg,
                        false,             // in boolean metaKeyArg,
                        null,              // key code;
                        val.charCodeAt(0)); // char code.

                    ctrl.dispatchEvent(e);
                }
            }
            else {
                var tmp = (document.selection && !window.opera) ? ctrl.value.replace(/\r/g, "") : ctrl.value;
                ctrl.value = tmp.substring(0, insertionS) + val + tmp.substring(insertionS, tmp.length);
            }

            setRange(ctrl, insertionS + val.length, insertionS + val.length);
        }

 //--></script>

    <title>Amana Capital investment portal secure login</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
</head>
<body onload="init()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div style="border: thin solid #000000; position: static; background-color: #00aeef;">
            <br />
            <table style="font-size: 8pt; font-family: Verdana">
                <tr style="border: thin solid #808000">
                    <td style="width: 227px;" align="left" valign="top">
                        <asp:Panel runat="server" GroupingText="Amana Capital IB" ID="Panel2" Width="100%" BackColor="Transparent"
                            Height="100%">
                            <asp:Image ID="Image1" runat="server" Height="177px" Width="227px" AlternateText="No seal" /><br />
                            <asp:TextBox ID="txtMessage" runat="server" ReadOnly="True" Width="221px"></asp:TextBox><br />
                            <asp:CheckBox ID="chkConfirmed" runat="server" Text="Please confirm your secure seal"
                                Font-Names="Arial" Font-Size="8pt" ForeColor="Blue" />
                            <table style="width: 227px">
                                <tr>
                                    <td style="width: 32%; text-align: right;">
                                        <asp:Label ID="Label1" runat="server" Text="User ID" Width="64px" Font-Bold="True"
                                            Font-Names="Arial" Font-Size="8pt"></asp:Label>
                                    </td>
                                    <td style="width: 158px;">
                                        <asp:TextBox ID="txtUserID" runat="server" Width="91px" ReadOnly="True" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 32%; text-align: right">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                            Text="Password" Width="64px"></asp:Label>
                                    </td>
                                    <td style="width: 158px">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="141px" onkeyup="getCaretPositions(this);"
                                            onclick="getCaretPositions(this);" MaxLength="15"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="text-align: right">
                                        <asp:Button ID="cmdLogin" runat="server" Text="Login" OnClick="cmdLogin_Click" Width="94px"
                                            Font-Bold="True" Font-Names="Arial" Font-Size="8pt" />
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Cancel" Width="105px"
                                            Font-Bold="True" Font-Names="Arial" Font-Size="8pt" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 41px; text-align: right" colspan="2" align="left">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Arial" Font-Size="8pt"
                                            OnClick="LinkButton1_Click" CausesValidation="False">Forgot Password</asp:LinkButton>
                                        |
                                        <asp:LinkButton ID="Btndownloadtoken" runat="server" OnClick="Btndownloadtoken_Click"
                                            Visible="False">Download Token</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                ForeColor="Red"></asp:Label></asp:Panel>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                    <td style="width: 80%;" valign="top">
                        <asp:Panel GroupingText="Introducing secure access to protect your online
                        transactions" runat="server" ID="Panel1" Width="100%" BackColor="Transparent" Font-Bold="False">
                            <span style="text-decoration: underline"></span>We are committed to ensure that
                            your online transactions are secure. Therefore we have introduced multi-layered
                            access authentication. You will have to create a personalized image(seal). This
                            seal enables you identify your genuine banking site from fradulent sites.&nbsp;
                            <%--<asp:Button ID="cmdReadMore" runat="server" OnClick="cmdReadMore_Click" Text="Read more"
                                Width="130px" Font-Names="Arial" Font-Size="8pt" ForeColor="Blue" Height="20px"
                                CausesValidation="False" />--%>
                            &nbsp; &nbsp; &nbsp;

                        </asp:Panel>
                        <asp:Panel GroupingText="Use virtual keyboard (highly recommended)" runat="server"
                            ID="Panel3" Width="100%" BackColor="Transparent" Height="100%">
                            <a href="javascript:keyb_change()" onclick="javascript:blur()" id="switch" style="font-family: Tahoma;
                                font-size: 14px; text-decoration: none; border-bottom: 1px dashed #0000F0; color: #0000F0">
                                Hide keyboard</a>
                            <br />
                            <div id="keyboard">
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </asp:Panel>
                        <br />
                        <br />
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Behavior="Close"
                            DestroyOnClose="True" InitialBehavior="Close" KeepInScreenBounds="True" singlenonminimizedwindow="True"
                            Skin="Office2007" Behaviors="Close" InitialBehaviors="Close" Left="" Top="">
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" title="Click to Verify - This site chose VeriSign SSL for secure e-commerce and confidential communications."
                style="width: 312px; height: 82px">
                <tr>
                    <td width="135" align="center" valign="top">

                        <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=ebank.equatorialbank.co.ke&amp;size=M&amp;use_flash=YES&amp;use_transparent=YES&amp;lang=en"></script>

                    </td>
                    <td width="155" valign="middle" style="font: 11px/16px Helvetica,Verdana,Arial,sans-serif;">
                        This site chose VeriSign <a href="http://www.verisign.com/ssl-certificate/" target="_blank">
                            SSL</a> for secure e-commerce and confidential communications.
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <a href="http://www.verisign.com/ssl-certificate/" target="_blank" style="color: #000000;
                            text-decoration: none; font: bold 7px verdana,sans-serif; letter-spacing: .5px;
                            text-align: center; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a>
                    </td>
                    <td width="155">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
