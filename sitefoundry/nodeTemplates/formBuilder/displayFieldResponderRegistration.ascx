<%@ Control Language="c#" AutoEventWireup="false" Codebehind="displayFieldResponderRegistration.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.displayFieldResponderRegistration" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h4><asp:literal id="title" runat="server" /></h4>
<p><asp:literal id="description" runat="server" /></p>

<h5>E-Mail: <asp:requiredfieldvalidator id="rfv_email" runat="server" controlToValidate="email" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="email" runat="server" maxlength="200" />


<h5>First Name: <asp:requiredfieldvalidator id="rfv_firstname" runat="server" controlToValidate="firstname" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="firstname" runat="server" maxlength="200" />

<h5>Last Name: <asp:requiredfieldvalidator id="rfv_lastname" runat="server" controlToValidate="lastname" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="lastname" runat="server" maxlength="200" />

<h5>Address: <asp:requiredfieldvalidator id="rfv_address" runat="server" controlToValidate="address1" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="address1" runat="server" maxlength="200" /><br/>
<asp:textbox id="address2" runat="server" maxlength="200" />

<h5>City: <asp:requiredfieldvalidator id="rfv_city" runat="server" controlToValidate="city" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="city" runat="server" maxlength="200" />

<h5>State / Province: <asp:comparevalidator id="rv_stateprov" runat="server" controlToValidate="stateprov" display="dynamic" cssclass="validatorDisplay" operator="notequal" valueToCompare="0" enabled="false">required</asp:comparevalidator></h5>
<asp:dropdownlist id="stateprov" runat="server" />

<h5>Postal Code: <asp:requiredfieldvalidator id="rfv_postcode" runat="server" controlToValidate="postcode" display="dynamic" cssclass="validatorDisplay" enabled="false">required</asp:requiredfieldvalidator></h5>
<asp:textbox id="postcode" runat="server" maxlength="200" />



