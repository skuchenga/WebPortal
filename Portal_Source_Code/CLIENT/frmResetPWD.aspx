<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmResetPWD.aspx.cs" Inherits="frmAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Amana Capital Portal | Forgot Password</title>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js">
 </script>
<link rel="stylesheet" type="text/css" media="screen" href="css/milk.css" />
<link rel="stylesheet" type="text/css" media="screen" href="css/jquery.validate.password.css" />
 <script type="text/javascript" src="js/jquery.js"></script>
<script type="text/javascript" src="js/jquery.validate.js"></script>
<script type="text/javascript" src="js/jquery.validate.password.js"></script>
         
  
 
<script id="demo" type="text/javascript">
 
$(document).ready(function() {
 

	// validate signup form on keyup and submit
	var validator = $("#signupform").validate({
		    rules: {
		     
		    txtUsername: {
			required: true
 			},
			  
			txtEmail: {
			required: true,
			email:true
 			} 
		},
		 
		// the errorPlacement has to take the table layout into account
		errorPlacement: function(error, element) {
			error.prependTo( element.parent().next() );
		},
		// specifying a submitHandler prevents the default submit, good for the demo
		submitHandler: function() {
		button.load();
 		},
		// set this class to error-labels to indicate valid fields
		success: function(label) {
			// set &nbsp; as text for IE
			label.html("&nbsp;").addClass("checked");
		}
	});
	
	// propose username by combining first- and lastname
	$("#username").focus(function() {
		var firstname = $("#firstname").val();
		var lastname = $("#lastname").val();
		if(firstname && lastname && !this.value) {
			this.value = firstname + "." + lastname;
		}
	});
	
});
 </script>

</head>
<body>

<div id="main">
   

<div id="content">
<div id="header" style="max-height:130px; padding-left: 130px;">
  <div id="headerlogo"><img src="Images/hfc_logo.jpg" alt="Amana Capital Portal"  /></div>
</div>
     <div id="signupbox">
      <div id="signupwrap">
      		<form id="signupform" autocomplete="off"  action="" runat="server" >
	  		  <table>
	  		  <tr>
	  			<td class="label"><label id="lusername" for="username">
                      User Name</label></td>
	  			<td class="field">
    <asp:TextBox ID="txtUsername" runat="server" Width="283px"></asp:TextBox>
    </td>
	  			<td class="status"></td>
	  		  </tr>
	  		  <tr>
	  			<td class="label" style="height: 29px"><label id="lpassword" for="password">
                      E-mail</label></td>
	  			<td class="field" style="height: 29px">
        <asp:TextBox ID="txtEmail" runat="server" Width="283px"></asp:TextBox>
    </td>
	  			<td class="status" style="height: 29px"> 
				</td>
	  		  </tr>
                    <tr>
                        <td class="label">
                        </td>
                        <td align="left" class="field">
                            <asp:Button ID="cmdSubmit" runat="server" OnClick="cmdSubmit_Click" Text="Submit" />
                            <asp:Button ID="cmdOk" runat="server" Text="Ok" />
                            </td>
                        <td class="status">
                        </td>
                    </tr>
	  		  <tr>
	  			<%--<td class="label"><label id="lsignupsubmit" for="signupsubmit"></label></td>--%>
	  			<td class="field" colspan="3">
                      <asp:Label ID="lblMessageLine" runat="server" Text="."></asp:Label></td>
	  		  </tr>

	  		  </table>
          </form>
      </div>
    </div>
 

</div>

</div>

<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
</script>
<script type="text/javascript">
_uacct = "UA-2623402-1";
urchinTracker();
</script>

</body>
</html>