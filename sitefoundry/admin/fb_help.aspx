bh<%@ Page language="c#" Codebehind="fb_help.aspx.cs" AutoEventWireup="false" Inherits="SiteFoundry.admin.fb_help" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Form Builder ~ Help</title>
<LINK href="includes/common.css" type=text/css rel=stylesheet ><LINK href="includes/admin.css" type=text/css rel=stylesheet >
<style type=text/css>BODY {
	MARGIN: 10px
}
P {
	MARGIN-BOTTOM: 10px
}
EM {
	FONT-WEIGHT: bold; FONT-STYLE: normal
}
H1 {
	MARGIN-BOTTOM: 10px
}
</style>
</HEAD>
<body>
<form id=Form1 method=post runat="server">
<h1>Form Builder Help</H1>
<h2>Getting Started</H2>
<p>When you first create a form, you must enter in some 
basic details before you can add questions. </P>
<blockquote>
  <p><em>Title:</EM> the title for 
  your form</P>
  <p><em>Description:</EM> This is the 
  text that will be displayed before the form. You can enter HTML code to 
  display images or for advanced formatting</P>
  <p><em>After Submission:</EM> This 
  is the text that will be displayed when the user can completed the form and 
  successfully submitted their information.</P></BLOCKQUOTE>
<h2>Adding Questions</H2>
<p>Before you can add questions or fields to your form, you 
must first create a <em>Section</EM>. Click the Add 
Question or Section button, then simply enter a <em>Title</EM> and optionally a <em>Description</EM> and then click the <em>Add Section.</EM> You may opt to have the section "hidden" 
by clicking off the <STRONG>display sections</STRONG> in 
Form Details. </P>
<P>Once you have a section created, you may add questions. 
</P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P><STRONG>Field Title:</STRONG> 
  this is the text that will appear before the form field</P>
  <P><STRONG>Section:</STRONG> which 
  section you would like the question to be created in</P>
  <P><STRONG>Type:</STRONG> the 
  question type:</P>
  <BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
    <P><STRONG>Text (Single 
    Line)</STRONG> - a single line text entry box</P>
    <P><STRONG>Text (Multi 
    Line)</STRONG> - a multiple line text entry box</P>
    <P><STRONG>Password </STRONG>- a 
    single line text entry box that masks the text typed in it</P>
    <P><STRONG>Checkbox </STRONG>- a 
    single checkbox used for on/off, yes/no type questions. Ex. Do you want want 
    to receive our monthly newsletter?</P>
    <P><STRONG>Checkboxlist </STRONG>- 
    multiple checkboxes that are related. Ex. What toppings do you want on your 
    pizza?</P>
    <P><STRONG>Radiobuttonlist 
    -</STRONG> multiple selectable items in a list that are mutually exclusive. 
    Ex. What colour do you want your t-shirt?</P>
    <P><STRONG 
    >Dropdownlist&nbsp;</STRONG>- a drop down select box</P>
    <P><STRONG 
    >ResponderRegistration</STRONG> - a form section in 
    which users can enter their address details. </P></BLOCKQUOTE>
  <P dir=ltr>Once you have selected a question type, click 
  <STRONG>add question.</STRONG></P></BLOCKQUOTE>
<P dir=ltr>You question will appear in the left panel 
titled "Current Fields in Form". Mouse over each question to make the edit tools 
appear. Just like the Node Editing tools, the <IMG src="images/up.gif" > and <IMG src="images/down.gif" > 
arrows will change the order of questions. Clicking the question title or the 
edit icon <IMG src="images/smalledit.gif" > will allow you to edit the question and clicking on the 
trash icon <IMG src="images/smalldelete.gif" > will allow you to 
delete it.</P>
<P dir=ltr>When editing a question, you can modify various 
properties to control the appearance and input options. </P>
<P dir=ltr><STRONG>Text (Single Line)</STRONG></P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr><STRONG>Title:</STRONG> text that appears 
  before the question</P>
  <P dir=ltr><STRONG>Description:</STRONG> optional 
  secondary line of text appearing before the question</P>
  <P dir=ltr><STRONG>Width:</STRONG> the width in pixels 
  that the text field occupies on the screen</P>
  <P dir=ltr><STRONG>Max Length:</STRONG> the maximum 
  number of characters allowed to be entered in the text box</P>
  <P dir=ltr><STRONG>Options:</STRONG></P>
  <BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
    <P dir=ltr><STRONG>Is Required:</STRONG> check if you'd 
    like the field to be mandatory</P>
    <P dir=ltr><STRONG>Is Validated:</STRONG> check if 
    you'd like the field to be validated against the Validation 
  Expression</P></BLOCKQUOTE>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG>Validation 
  Expression:</STRONG> a Regular Expression by which the text entered must match 
  to be valid if you've check Is Validated</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px">&nbsp;</P></BLOCKQUOTE>
<P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG>Text (Multi Line)</STRONG> - As above, with one additional 
option</P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Rows:</STRONG> the number of rows displayed by the 
  multiline textbox</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px">&nbsp;</P></BLOCKQUOTE>
<P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG>Password </STRONG>- options are the same as Text (Single 
Line)</P>
<P dir=ltr style="MARGIN-RIGHT: 0px">&nbsp;</P>
<P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG>Checkbox</STRONG></P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Title:</STRONG> text that appears before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Description:</STRONG> Description: optional secondary 
  line of text appearing before the question</P>
  <P dir=ltr><STRONG>Is 
  Required:</STRONG> Check if you would&nbsp;like the field to be 
mandatory</P></BLOCKQUOTE>
<P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG>CheckBoxList, RadioButtonList</STRONG>&nbsp; </P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Title:</STRONG> text that appears before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Description:</STRONG> Description: optional secondary 
  line of text appearing before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Items:</STRONG> The items you would like to appear in the 
  list. See Formatting Items for more information</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Repeat Direction:</STRONG> Which direction you would like 
  the list of CheckBoxes or RadioButtons&nbsp;to repeat in</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Is Required:</STRONG> Check if you would like this field 
  to be mandatory</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px">&nbsp;</P></BLOCKQUOTE>
<P><STRONG>DropDownList</STRONG></P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Title:</STRONG> text that appears before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Description:</STRONG> Description: optional secondary 
  line of text appearing before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Items:</STRONG> The items you would like to appear in the 
  list. See Formatting Items for more information</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Width:</STRONG> The width in pixels you would like the 
  dropdown to occupy</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Is Required:</STRONG> Check if you would like this field 
  to be mandatory</P></BLOCKQUOTE>
<P>&nbsp;</P>
<P><STRONG>Responder 
Registration</STRONG></P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Title:</STRONG> text that appears before the question</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Description:</STRONG> Description: optional secondary 
  line of text appearing before the registration fields</P>
  <P dir=ltr style="MARGIN-RIGHT: 0px"><STRONG 
  >Is Required:</STRONG> Check if you would like these 
  fields to be mandatory</P></BLOCKQUOTE>
<P>&nbsp;</P>
<P><STRONG>Formatting 
Items</STRONG></P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
  <P>Items in the CheckBoxList, RadioButtonList or 
  DropDownList are all formatted in the same way. Each Item can have a Label and 
  a Value. The Label is what will appear to the user and the Value is what will 
  be stored in the DataBase. You don't have to have a Value, in that case, the 
  Label is stored in the DataBase. </P>
  <P>By using special characters, you can populate the 
  question with your desired items. Each item is separated by a comma "," and 
  the Label and optional Value are separated by the pipe "|" character.</P>
  <P>Examples:</P>
  <P dir=ltr>
  <TABLE id='Table1"' border=1>
    <TR>
      <TD>blue,green,red,yellow </TD>
      <TD><asp:radiobuttonlist id=RadioButtonList1 runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
<asp:ListItem Value="blue">blue</asp:ListItem>
<asp:ListItem Value="green">green</asp:ListItem>
<asp:ListItem Value="red">red</asp:ListItem>
<asp:ListItem Value="yellow">yellow</asp:ListItem>
</asp:radiobuttonlist></TD></TR>
    <TR>
      <TD>the first item|1,the second item|2,the third 
        item|3</TD>
      <TD><asp:dropdownlist id=DropDownList1 runat="server">
<asp:ListItem Value="the first item">the first item</asp:ListItem>
<asp:ListItem Value="the second item">the second item</asp:ListItem>
<asp:ListItem Value="the third item">the third item</asp:ListItem>
</asp:dropdownlist></TD></TR>
    <TR>
        <TD></TD>
        <TD></TD></TR></TABLE></P>
  <BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
    <P>&nbsp;</P>
    <P>&nbsp;</P>
    <P>&nbsp;</P>
    <P>&nbsp;</P></BLOCKQUOTE></BLOCKQUOTE>
<P>&nbsp;</P></FORM>
	
  </body>
</HTML>
