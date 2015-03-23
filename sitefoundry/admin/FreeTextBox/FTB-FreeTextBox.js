
/* main FTB object
-------------------------------------- */
function FTB_FreeTextBox(id, enableToolbars, readOnly, buttons, dropdownlists, breakMode, pasteMode, tabMode, startMode, clientSideTextChanged, designModeCss, baseUrl, buttonImageFormat, imageGalleryUrl, imageGalleryPath, receiveFocus, buttonWidth, buttonHeight) {
	this.debug = document.getElementById('debug');
	
	// 1. Setup variables
	this.id = id;
	this.enableToolbars = enableToolbars;
	this.readOnly = readOnly;
	this.buttons = buttons;
	this.dropdownlists = dropdownlists;
	this.breakMode = breakMode;
	this.pasteMode = pasteMode;
	this.tabMode = tabMode;
	this.clientSideTextChanged = clientSideTextChanged;

	this.designModeCss = designModeCss;
	this.baseUrl = baseUrl;
	this.buttonImageFormat = buttonImageFormat; // currently unused
	this.imageGalleryUrl = imageGalleryUrl;
	this.imageGalleryPath = imageGalleryPath;	
	this.hasFocus = false;
	this.mode = FTB_MODE_DESIGN;
	this.initialized = false;
	this.undoArray = new Array();
	this.undoArrayMax = 16;
	this.undoArrayPos = -1;
 
	var ftb = this;

	// 2. Find everything
	//* windows
	this.htmlEditor = document.getElementById(id);

	if (FTB_Browser.isIE) {
		this.previewPane = eval(id + "_previewPane");
		this.designEditor = eval(id + "_designEditor");
		this.designEditor.ftb = this;
		this.designEditor.document.ftb = this;
		document.getElementById(id + "_designEditor").document.ftb = this;
	} else {
		this.previewPane = document.getElementById(id + "_previewPane").contentWindow;
		this.designEditor = document.getElementById(id + "_designEditor").contentWindow;
		this.designEditor.document.ftb = this;		
	}

	//* areas
	this.toolbarArea = document.getElementById(this.id + "_toolbarArea");
	this.designEditorArea = document.getElementById(this.id + "_designEditorArea");
	this.htmlEditorArea = document.getElementById(this.id + "_htmlEditorArea");
	this.previewPaneArea = document.getElementById(this.id + "_previewPaneArea");

	//* tabs
	this.designModeTab = document.getElementById(this.id + "_designModeTab");
	if (this.designModeTab) {
		this.designModeTab.ftb = this;
		this.designModeTab.onclick = function() { this.ftb.GoToDesignMode(); this.ftb.Focus(); this.ftb.UpdateToolbars(); }
	}

	this.htmlModeTab = document.getElementById(this.id + "_htmlModeTab");
	if (this.htmlModeTab) {
		this.htmlModeTab.ftb = this;
		this.htmlModeTab.onclick = function() { this.ftb.GoToHtmlMode(); this.ftb.Focus();  this.ftb.UpdateToolbars();}
	}

	this.previewModeTab = document.getElementById(this.id + "_previewModeTab");
	if (this.previewModeTab) {
		this.previewModeTab.ftb = this;
		this.previewModeTab.onclick = function() { this.ftb.GoToPreviewMode();}
	}
	
	//* ancestor area
	this.ancestorArea = document.getElementById(this.id + "_AncestorArea");

	// 3. Tell buttons who owns them
	//* setup buttons & dropdowns
	if (this.enableToolbars) {
		for(var i=0; i<this.buttons.length; i++) {
			button = buttons[i];
			button.ftb = this;
		}
		for(var i=0; i<this.dropdownlists.length; i++) {
			dropdownlist = dropdownlists[i];
			dropdownlist.ftb = this;
		}
	}
       
	
	// 4. Setup editor for use
	if (!this.readOnly) {
		this.designEditor.document.designMode = 'On';
		if (FTB_Browser.isGecko) this.designEditor.document.contentEditable = true;
		
  		// strangely this must happen here and at no other spot!
  		if (FTB_Browser.isGecko)
    		this.designEditor.document.execCommand("useCSS", false, true);
   	}
	this.designEditor.document.open();
	this.designEditor.document.write("<html><head>" + ((this.designModeCss != '') ? "<link rel='stylesheet' href='" + this.designModeCss + "' type='text/css' />" : "") + ((this.baseUrl != '') ? "<base href='" + this.baseUrl + "' />" : "") + "</head><body>" + this.htmlEditor.value + "</body></html>");
	this.designEditor.document.close();
	
	if (!this.readOnly) {	
		if (FTB_Browser.isIE) this.designEditor.document.body.contentEditable = 'True';
		
		// enable this html area
		this.htmlEditor.disabled = '';
	}	
	
	// silly IE can't get the style right until now...
	if (FTB_Browser.isIE) {
		this.designEditor.document.body.style.border = '0';		
		//if (designModeCss != '') 
		//	this.designEditor.document.createStyleSheet(designModeCss);		
	}
	
	// 5. Add events
    //* toolbar updates
    if (!this.readOnly) {
		if (FTB_Browser.isIE) {
			FTB_AddEvents(this.designEditor.document,
				new Array("keydown","click"),
				function(e) { ftb.hasFocus=true; ftb.Event(e); } 
			);
			
			FTB_AddEvents(this.designEditor.document.body,
				new Array("paste"),
				function(e) { return this.document.ftb.CapturePaste(e); } 
			);
		
		} else {
			var evt = function(e) {	
				this.document.ftb.hasFocus=true; 
				return this.document.ftb.Event(e);
			}
			this.designEditor.addEventListener("keydown", evt, true);
			this.designEditor.addEventListener("mousedown", evt, true);
			this.designEditor.addEventListener("click", evt, true);

      		// no paste event in Mozilla
		}
		FTB_AddEvents(this.designEditor,
			new Array("blur"),
			function(e) { ftb.hasFocus=false; ftb.Event(e); return ftb.StoreHtml(); } 
		);
	}
     	
	if (startMode == FTB_MODE_HTML)
		this.GoToHtmlMode();
		
	if (this.readOnly) 
		this.DisableAllToolbarItems();
	else
		this.UpdateToolbars();
	
	//this.undoArray[0] = this.htmlEditorArea.value;
	this.initialized = true;
	if (receiveFocus) this.Focus();
}

FTB_FreeTextBox.prototype.AddStyle = function(css) {
	var styleEl=document.createElement('style');
	styleEl.type='text/css';
	styleEl.appendChild(css);
	this.designEditor.document.appendChild(styleEl);
}

FTB_FreeTextBox.prototype.Event = function(ev) {
 	if (ev != null) { 	
 		sel = this.GetSelection();
 		r = this.CreateRange(sel);
 		if (FTB_Browser.isIE) {
 			// break mode code
	 		
	 		
 			// check for undo && redo
 			if (ev.ctrlKey && ev.keyCode == FTB_KEY_Z) {
 				this.Undo();
				this.CancelEvent(ev);			
 			} else if (ev.ctrlKey && ev.keyCode == FTB_KEY_Y) { 			
 				this.Redo(); 
				this.CancelEvent(ev);
 			} else {
		 		
 				if (ev.keyCode == FTB_KEY_ENTER) {
 					if (this.breakMode == FTB_BREAK_BR || ev.ctrlKey) {
						if (sel.type == 'Control') {
							return;
						}
						if ((!this.CheckTag(r.parentElement(),'LI'))&&(!this.CheckTag(r.parentElement(),'H'))) {
							r.pasteHTML('<br>');
							this.CancelEvent(ev);
							r.select();
							r.collapse(false);
							return false;
						} 			
 					} 		
 				} else if (ev.keyCode == FTB_KEY_TAB) {	
	 				if (this.CheckTag(r.parentElement(),'LI')) {
	 					if (ev.shiftKey)
	 						this.ExecuteCommand("outdent");
	 					else
	 						this.ExecuteCommand("indent");
	 					this.CancelEvent(ev);
	 				} else {	 				
	 					switch (this.tabMode) {
	 						default:
	 						case FTB_TAB_NEXTCONTROL:
	 							break;
	 						case FTB_TAB_INSERTSPACES:
	 							this.InsertHtml("&nbsp;&nbsp;&nbsp;");
	 							this.CancelEvent(ev);
	 							break;
	 						case FTB_TAB_DISABLED:
	 							this.CancelEvent(ev);
	 							break;	 						
	 					}
	 				}
 				}
 			}
	 	
 		} else { 	 	
	 		
 			// check for undo && redo
 			if (ev.ctrlKey && ev.which && ev.which == FTB_KEY_Z) {	 			
 				this.Undo();
 				this.CancelEvent(ev);		
 			} else if (ev.ctrlKey && ev.which && ev.which == FTB_KEY_Y) {	 			
 				this.Redo(); 
 				this.CancelEvent(ev);		
 			} else {
 	 			
 				// if not undo or redo, then keep going
	 		
 				// check for control+commands (not in Mozilla by default)
 				if ((ev.ctrlKey && !ev.shiftKey && !ev.altKey)) {
 					if (ev.which == FTB_KEY_P) {  											
 						this.CapturePaste(ev); 		 						
 						this.CancelEvent(ev);
 					} else if (ev.which == FTB_KEY_B) { 						
 						this.ExecuteCommand('bold');
 						this.CancelEvent(ev);
 					} else if (ev.which == FTB_KEY_I) { 						
 						this.ExecuteCommand('italic');				
 						this.CancelEvent(ev);
 					} else if (ev.which == FTB_KEY_I) { 						
 						this.ExecuteCommand('underline');				 						
 						this.CancelEvent(ev);
 					}
  				} else if (ev.which == FTB_KEY_TAB) {
  			 		if (this.CheckTag(r.parentElement(),'LI')) {
	 					// do it's own thing!
	 				} else {	 				
	 					switch (this.tabMode) {
	 						default:
	 						case FTB_TAB_NEXTCONTROL:
	 							// unsupported in Mozilla
	 							break;
	 						case FTB_TAB_INSERTSPACES:
	 							// do it's own thing
	 							break;
	 						case FTB_TAB_DISABLED:
	 							this.CancelEvent(ev);
	 							break;	 						
	 					}
	 				}			
  				
  				}
  			} 	
 		}
 	}
 
	if (this.mode == FTB_MODE_DESIGN) {
		FTB_Timeout.addMethod(this.id+'_UpdateToolbars',this,'UpdateToolbars',200);
	}
	
	if (this.clientSideTextChanged)
		this.clientSideTextChanged(this);
}
FTB_FreeTextBox.prototype.CancelEvent = function(ev) {
	if (FTB_Browser.isIE) {
		ev.cancelBubble = true;
		ev.returnValue = false;
	} else {
		ev.preventDefault();
		ev.stopPropagation();
	}
}

/*
FTB_FreeTextBox.prototype.RecordUndoStep = function() {	
	while (this.undoArrayPos > this.undoArray.length)
		this.undoArray.pop();
	
	this.undoArray.push(this.designEditor.document.body.innerHTML);
	
	if (this.undoArray.length > this.undoArrayMax)
		this.undoArray.shift();
	else
		this.undoArrayPos++;
}
*/
FTB_FreeTextBox.prototype.RecordUndoStep = function() {	

	++this.undoArrayPos;
	if (this.undoArrayPos >= this.undoArrayMax) {
		// remove the first element
		this.undoArray.shift();
		--this.undoArrayPos;
	}
	// use the fasted method (getInnerHTML);
	var take = true;
	var html = this.designEditor.document.body.innerHTML;
	if (this.undoArrayPos > 0)
		take = (this.undoArray[this.undoArrayPos - 1] != html);
	if (take) {
		this.undoArray[this.undoArrayPos] = html;
	} else {
		this.undoArrayPos--;
	}
}

FTB_FreeTextBox.prototype.Undo = function() {
	if (this.undoArrayPos > 0) {
		var html = this.undoArray[--this.undoArrayPos];
		if (html)
			this.designEditor.document.body.innerHTML = html;
		else 
			++this.undoArrayPos;
	}
}
FTB_FreeTextBox.prototype.CanUndo = function() {
	return true;
	return (this.undoArrayPos > 0);
}
FTB_FreeTextBox.prototype.Redo = function() {
	if (this.undoArrayPos < this.undoArray.length - 1) {
		var html = this.undoArray[++this.undoArrayPos];
		if (html) 
			this.designEditor.document.body.innerHTML = html;
		else 
			--this.undoArrayPos;
	}
	
}
FTB_FreeTextBox.prototype.CanRedo = function() {
	return true;
	return (this.undoArrayPos < this.undoArray.length - 1);
}



FTB_FreeTextBox.prototype.CapturePaste = function(ev) {
	return true;
 	switch (this.pasteMode) {
 		case FTB_PASTE_DISABLED:
 			return false;
 		case FTB_PASTE_TEXT:
			var text = window.clipboardData.getData('Text');
			text = text.replace(/<[^>]*>/gi,'');
			this.InsertHtml(text);
			return false; 		
 		default:
 		case FTB_PASTE_DEFAULT:
 			// do nothing
 			return true;
 	}		
}

FTB_FreeTextBox.prototype.Debug = function(text) {
	if (this.debug)
		this.debug.value += text + '\r';
}

FTB_FreeTextBox.prototype.UpdateToolbars = function() {
	
	if (this.hasFocus) {

		if (this.mode == FTB_MODE_DESIGN) {
			if (this.enableToolbars) {
				for (var i=0; i<this.buttons.length; i++) {
					button = this.buttons[i];

					if (button.customStateQuery)
						button.state = button.customStateQuery();			
					else if (button.commandIdentifier != null && button.commandIdentifier != '')
						button.state = this.QueryCommandState(button.commandIdentifier);

					button.SetButtonBackground("Out");
				}
				for (var i=0; i<this.dropdownlists.length; i++) {
					dropdownlist = this.dropdownlists[i];

					if (dropdownlist.customStateQuery)
						dropdownlist.SetSelected(dropdownlist.customStateQuery());
					else if (dropdownlist.commandIdentifier != null && dropdownlist.commandIdentifier != '') 
						dropdownlist.SetSelected(this.QueryCommandValue(dropdownlist.commandIdentifier));

				}
			}
			this.UpdateAncestorTrail();
		}	
		
	} else {
	
		if (this.enableToolbars) {
			for (var i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];
				button.state = FTB_BUTTON_OFF;
				button.SetButtonBackground("Out");
			}
			for (var i=0; i<this.dropdownlists.length; i++) {
				dropdownlist = this.dropdownlists[i];
				dropdownlist.list.selectedIndex = 0;
			}
		}
		this.UpdateAncestorTrail();		
	}

	if (!this.undoTimer) {
		this.RecordUndoStep();
		var editor = this;
		this.undoTimer = setTimeout(function() {
			editor.undoTimer = null;
		}, 500);
	}
	
	this.SetToolbarItemsEnabledState();	
	
	if (this.timerToolbar) 
		this.timerToolbar = null;	
}

FTB_FreeTextBox.prototype.UpdateAncestorTrail = function() {	
	if (this.ancestorArea)  {
	
		if (this.hasFocus) {
			ancestors = this.GetAllAncestors();

			this.ancestorArea.innerHTML = "Path(" + ancestors.length + "): ";	

			for (var i = ancestors.length-1; i>-1; i--) {
				var el = ancestors[i];
				if (!el) {
					continue;
				}
				var a = document.createElement("a");
				a.href = "javascript:void();";
				a.el = el;
				a.ftb = this;
				a.onclick = function() {
					this.blur();
					this.ftb.SelectNodeContents(this.el);
					this.ftb.UpdateToolbars();
					return false;
				};
				a.oncontextmenu = function () {
					this.ftb.EditElementStyle(this.el);
					return false;
				}

				var txt = el.tagName.toLowerCase();
				if (txt == "input") txt = el.type;
				a.title = el.style.cssText;
				if (el.id) {
					txt += "#" + el.id;
				}
				if (el.className) {
					txt += "." + el.className;
				}
				a.appendChild(document.createTextNode("<" + txt + ">"));
				this.ancestorArea.appendChild(a);
				//if (i != 0)
				//	this.ancestorArea.appendChild(document.createTextNode(String.fromCharCode(0xbb)));

			}
		} else {
			this.ancestorArea.innerHTML = "";
		}
	}
}

FTB_FreeTextBox.prototype.SetToolbarItemsEnabledState = function() {	
	if (!this.enableToolbars) return;
	if (this.hasFocus) {
	
		if (this.mode == FTB_MODE_DESIGN ) {		
			for (i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];

				if (button.customEnabled)
					button.customEnabled();
				else 
					button.disabled = false;

				if (button.disabled)
					this.DisableButton(button);			
				else
					this.EnableButton(button);
			}

			for (i=0; i<this.dropdownlists.length; i++) {
				this.dropdownlists[i].list.disabled=false;
			}		
		} else {
			for (i=0; i<this.buttons.length; i++) {
				button = this.buttons[i];

				if (button.htmlModeEnabled) 
					button.disabled=false
				else
					button.disabled = true;

				if (button.disabled)
					this.DisableButton(button);			
				else
					this.EnableButton(button);
			}

			for (i=0; i<this.dropdownlists.length; i++) {
				this.dropdownlists[i].list.selectedIndex=0;
				this.dropdownlists[i].list.disabled=true;
			}	
		}
	} else {
		// do nothing: uncomment code to disable buttons when the editor does not have focus
		/*
		for (i=0; i<this.buttons.length; i++)
			this.DisableButton(this.buttons[i]);			

		for (i=0; i<this.dropdownlists.length; i++)
			this.dropdownlists[i].list.disabled=true;
		*/
	}
}

FTB_FreeTextBox.prototype.DisableAllToolbarItems = function() {	
	if (this.enableToolbars) {
		for (i=0; i<this.buttons.length; i++) {
			this.DisableButton(this.buttons[i]);			
		}

		for (i=0; i<this.dropdownlists.length; i++) {
			this.dropdownlists[i].list.disabled=true;
		}
	}
}

FTB_FreeTextBox.prototype.EnableButton = function(button) {
	if (FTB_Browser.isIE)
		button.buttonImage.style.filter = "alpha(opacity = 100);";
		//button.td.style.filters.alpha.opacity = 100;
	else 
		button.buttonImage.style.MozOpacity = 1;
}

FTB_FreeTextBox.prototype.DisableButton = function(button) {
	button.state = FTB_BUTTON_OFF;
	button.SetButtonStyle("Out");

	if (FTB_Browser.isIE)
		button.buttonImage.style.filter = "alpha(opacity = 25);";
		//button.td.style.filters.alpha.opacity = 25;		
	else 
		button.buttonImage.style.MozOpacity = 0.25;
}


FTB_FreeTextBox.prototype.CopyHtmlToIframe = function(iframe) {
   	if (this.initialized) {
		html = this.htmlEditor.value;
		iframe.document.body.innerHTML = html;
   	} else {
		iframe.document.open();
		iframe.document.write("<html><head><link rel='stylesheet' href='" + this.designModeCss + "' type='text/css' /></head><body>" + this.htmlEditor.value + "</body></html>");
		//iframe.document.write(this.htmlEditor.value);
		iframe.document.close();
	}
}

FTB_FreeTextBox.prototype.CopyDesignToHtml = function() {
	this.htmlEditor.value = this.designEditor.document.body.innerHTML;
}

FTB_FreeTextBox.prototype.GoToHtmlMode = function() {
    if (this.mode == FTB_MODE_DESIGN) this.CopyDesignToHtml();

    this.designEditorArea.style.display = 'none';
    this.htmlEditorArea.style.display = '';
    this.previewPaneArea.style.display = 'none';
   
	if (this.ancestorArea) this.ancestorArea.innerHTML = "";    
    this.SetActiveTab(this.htmlModeTab);    
         
	this.mode = FTB_MODE_HTML;
    //this.Focus();    
    return true;
}

FTB_FreeTextBox.prototype.GoToDesignMode = function() {
	if (this.mode == FTB_MODE_DESIGN) return false;
   
   	this.CopyHtmlToIframe(this.designEditor);
	this.designEditorArea.style.display = '';
	this.htmlEditorArea.style.display = 'none';
	this.previewPaneArea.style.display = 'none';	

	// reset for Gecko
	this.designEditor.document.designMode = 'On';
	if (FTB_Browser.isGecko) this.designEditor.document.execCommand("useCSS", false, true);
    
    if (this.ancestorArea) this.ancestorArea.innerHTML = "";
    
    this.SetActiveTab(this.designModeTab);
    
    //this.SetToolbarItemsEnabledState();
    
    this.mode = FTB_MODE_DESIGN;
    //this.Focus();
    return true;
}

FTB_FreeTextBox.prototype.GoToPreviewMode = function() {
    if (this.mode == FTB_MODE_DESIGN) this.CopyDesignToHtml();
    this.CopyHtmlToIframe(this.previewPane);

    this.designEditorArea.style.display = 'none';
    this.htmlEditorArea.style.display = 'none';
    this.previewPaneArea.style.display = '';
      
    this.SetActiveTab(this.previewModeTab);
    if (this.ancestorArea) this.ancestorArea.innerHTML = "";
    
    this.mode = FTB_MODE_PREVIEW;
    return true;
}
FTB_FreeTextBox.prototype.HtmlEncode = function( text ) {
	if ( typeof( text ) != "string" )
		text = text.toString() ;

	text = text.replace(/&/g, "&amp;") ;
	text = text.replace(/"/g, "&quot;") ;
	text = text.replace(/</g, "&lt;") ;
	text = text.replace(/>/g, "&gt;") ;
	text = text.replace(/'/g, "&#146;") ;

	return text ;
}

FTB_FreeTextBox.prototype.ExecuteCommand = function(commandName, middle, commandValue) {
	if (this.mode != FTB_MODE_DESIGN) return;
	this.designEditor.focus();
	if (commandName == 'backcolor' && !FTB_Browser.isIE) commandName = 'hilitecolor';
	this.designEditor.document.execCommand(commandName,middle,commandValue);
	if (this.clientSideTextChanged)
		this.clientSideTextChanged(this);
}
FTB_FreeTextBox.prototype.QueryCommandState = function(commandName) {
	if (this.mode != FTB_MODE_DESIGN) return false;
	try {
		if (this.designEditor.document.queryCommandState(commandName)) {
			return FTB_BUTTON_ON;
		} else {
			// special case for paragraph on IE
			if (commandName == 'justifyleft') {
				if (this.designEditor.document.queryCommandState('justifyright') == false &&
					this.designEditor.document.queryCommandState('justifycenter') == false &&
					this.designEditor.document.queryCommandState('justifyfull') == false ) {
					return FTB_BUTTON_ON;
				} else {
					return FTB_BUTTON_OFF;
				}
			} else { 
				return FTB_BUTTON_OFF;
			}
		}
	} catch(exp) {
		return FTB_BUTTON_OFF;
	}
}
FTB_FreeTextBox.prototype.QueryCommandValue = function(commandName) {
	if (this.mode != FTB_MODE_DESIGN) return false;
	value = this.designEditor.document.queryCommandValue(commandName);
	switch (commandName) {
		case "backcolor":
			if (FTB_Browser.isIE) {
				value = FTB_IntToHexColor(value);
			} else {
				if (value == "") value = "#FFFFFF";
			}
			break;
		case "forecolor":
			if (FTB_Browser.isIE) {
				value = FTB_IntToHexColor(value);
			} else {
				if (value == "") value = "#000000";
			}
			break;
		case "formatBlock":
			if (!FTB_Browser.isIE) {
				if (value == "" || value == "<x>")
					value = "<p>";
				else
					value = "<" + value + ">";
			}
			break;
	}
	if (value == '' || value == null) {
		if (commandName == 'fontsize') return '3';
		if (commandName == 'fontname') return 'Times New Roman';	
		if (commandName == 'forecolor') return '#000000';	
		if (commandName == 'backcolor') return '#ffffff';
	}
		
	return value;

}

FTB_FreeTextBox.prototype.SurroundHtml = function(start,end) {
	if (this.mode == FTB_MODE_HTML) return;
	this.designEditor.focus();
	
	if (FTB_Browser.isIE) {
		var sel = this.designEditor.document.selection.createRange();
		html = start + sel.htmlText + end;
		sel.pasteHTML(html);		
	} else {
        selection = this.designEditor.window.getSelection();
        if (selection) {
            range = selection.getRangeAt(0);
        } else {
            range = this.designEditor.document.createRange();
        } 
        
        this.InsertHtml(start + selection + end);
	}	
}

FTB_FreeTextBox.prototype.InsertHtml = function(html) {
	if (this.mode != FTB_MODE_DESIGN) return;
	this.designEditor.focus();
	if (FTB_Browser.isIE) {
		sel = this.designEditor.document.selection.createRange();
		sel.pasteHTML(html);
	} else {

        selection = this.designEditor.window.getSelection();
		if (selection) {
			range = selection.getRangeAt(0);
		} else {
			range = editor.document.createRange();
		}

        var fragment = this.designEditor.document.createDocumentFragment();
        var div = this.designEditor.document.createElement("div");
        div.innerHTML = html;

        while (div.firstChild) {
            fragment.appendChild(div.firstChild);
        }

        selection.removeAllRanges();
        range.deleteContents();

        var node = range.startContainer;
        var pos = range.startOffset;

        switch (node.nodeType) {
            case 3:
                if (fragment.nodeType == 3) {
                    node.insertData(pos, fragment.data);
                    range.setEnd(node, pos + fragment.length);
                    range.setStart(node, pos + fragment.length);
                } else {
                    node = node.splitText(pos);
                    node.parentNode.insertBefore(fragment, node);
                    range.setEnd(node, pos + fragment.length);
                    range.setStart(node, pos + fragment.length);
                }
                break;

            case 1:
                node = node.childNodes[pos];
                node.parentNode.insertBefore(fragment, node);
                range.setEnd(node, pos + fragment.length);
                range.setStart(node, pos + fragment.length);
                break;
        }
        selection.addRange(range);
	}
}

/* ------------------------------------------------
START: Node and Selection Methods */

FTB_FreeTextBox.prototype.CheckTag = function(item,tagName) {
	if (item.tagName.search(tagName)!=-1) {
		return item;
	}
	if (item.tagName=='BODY') {
		return false;
	}
	item=item.parentElement;
	return this.CheckTag(item,tagName);
}

FTB_FreeTextBox.prototype.GetParentElement = function() {

	var sel = this.GetSelection();
	var range = this.CreateRange(sel);
	if (FTB_Browser.isIE) {
		switch (sel.type) {
		    case "Text":
		    case "None":
				return range.parentElement();
		    case "Control":
				return range.item(0);
		    default:
				return this.designEditor.document.body;
		}
	} else try {
		var p = range.commonAncestorContainer;
		if (!range.collapsed && range.startContainer == range.endContainer &&
		    range.startOffset - range.endOffset <= 1 && range.startContainer.hasChildNodes())
			p = range.startContainer.childNodes[range.startOffset];
		/*
		alert(range.startContainer + ":" + range.startOffset + "\n" +
		      range.endContainer + ":" + range.endOffset);
		*/
		while (p.nodeType == 3) {
			p = p.parentNode;
		}
		return p;
	} catch (e) {
		return null;
	}
}

FTB_FreeTextBox.prototype.InsertNodeAtSelection = function(toBeInserted) {
	if (!FTB_Browser.isIE) {
		var sel = this.GetSelection();
		var range = this.CreateRange(sel);
		// remove the current selection
		sel.removeAllRanges();
		range.deleteContents();
		var node = range.startContainer;
		var pos = range.startOffset;
		switch (node.nodeType) {
		    case 3: // Node.TEXT_NODE
			// we have to split it at the caret position.
			if (toBeInserted.nodeType == 3) {
				// do optimized insertion
				node.insertData(pos, toBeInserted.data);
				range = this._createRange();
				range.setEnd(node, pos + toBeInserted.length);
				range.setStart(node, pos + toBeInserted.length);
				sel.addRange(range);
			} else {
				node = node.splitText(pos);
				var selnode = toBeInserted;
				if (toBeInserted.nodeType == 11 /* Node.DOCUMENT_FRAGMENT_NODE */) {
					selnode = selnode.firstChild;
				}
				node.parentNode.insertBefore(toBeInserted, node);
				this.SelectNodeContents(selnode);
			}
			break;
		    case 1: // Node.ELEMENT_NODE
			var selnode = toBeInserted;
			if (toBeInserted.nodeType == 11 /* Node.DOCUMENT_FRAGMENT_NODE */) {
				selnode = selnode.firstChild;
			}
			node.insertBefore(toBeInserted, node.childNodes[pos]);
			this.SelectNodeContents(selnode);
			break;
		}
	}
}

FTB_FreeTextBox.prototype.SelectNodeContents = function(node, pos) {
	var range;
	var collapsed = (typeof pos != "undefined");
	if (isIE) {
		range = this.designEditor.document.body.createTextRange();
		range.moveToElementText(node);
		(collapsed) && range.collapse(pos);
		range.select();
	} else {
		var sel = this.GetSelection();
		range = this.designEditor.document.createRange();
		range.selectNodeContents(node);
		(collapsed) && range.collapse(pos);
		sel.removeAllRanges();
		sel.addRange(range);
	}
}
FTB_FreeTextBox.prototype.SelectNextNode = function(el) {
	var node = el.nextSibling;
	while (node && node.nodeType != 1) {
		node = node.nextSibling;
	}
	if (!node) {
		node = el.previousSibling;
		while (node && node.nodeType != 1) {
			node = node.previousSibling;
		}
	}
	if (!node) {
		node = el.parentNode;
	}
	this.SelectNodeContents(node);
}
FTB_FreeTextBox.prototype.GetSelection = function() {
	if (FTB_Browser.isIE) {
		return this.designEditor.document.selection;
	} else {
		return this.designEditor.getSelection();
	}
}
FTB_FreeTextBox.prototype.CreateRange = function(sel) {
	if (FTB_Browser.isIE) {
		return sel.createRange();
	} else {
		if (typeof sel != "undefined") {
			try {
				return sel.getRangeAt(0);
			} catch(e) {
				return this.designEditor.document.createRange();
			}
		} else {
			return this.designEditor.document.createRange();
		}
	}
}

FTB_FreeTextBox.prototype.SelectNodeContents = function(node, pos) {
	var range;
	var collapsed = (typeof pos != "undefined");
	if (FTB_Browser.isIE) {
		range = this.designEditor.document.body.createTextRange();
		range.moveToElementText(node);
		(collapsed) && range.collapse(pos);
		range.select();
	} else {
		var sel = this.GetSelection();
		range = this.designEditor.document.createRange();
		range.selectNodeContents(node);
		(collapsed) && range.collapse(pos);
		sel.removeAllRanges();
		sel.addRange(range);
	}
}
FTB_FreeTextBox.prototype.GetNearest = function(tagName) {
	var ancestors = this.GetAllAncestors();
	var ret = null;
	tagName = ("" + tagName).toLowerCase();
	for (var i=0;i<ancestors.length;i++) {
		var el = ancestors[i];
		if (el) {
			if (el.tagName.toLowerCase() == tagName) {
				ret = el;
				break;
			}
		}
	}
	return ret;
}
FTB_FreeTextBox.prototype.GetAllAncestors = function() {
	var p = this.GetParentElement();
	var a = [];
	while (p && (p.nodeType == 1) && (p.tagName.toLowerCase() != 'body')) {
		a.push(p);
		p = p.parentNode;
	}
	a.push(this.designEditor.document.body);
	return a;
}


FTB_FreeTextBox.prototype.GetStyle = function() {
	var parent = this.GetParentElement();
	return parent.className;	
}

FTB_FreeTextBox.prototype.SetActiveTab = function(theTD) {
	if (theTD) {
		parentTR = theTD.parentElement;
		parentTR = document.getElementById(this.id + "_TabRow");

		selectedTab = 1;
		totalButtons = parentTR.cells.length-1;
		for (var i=1;i< totalButtons;i++) {
			parentTR.cells[i].className = this.id + "_TabOffRight";
			if (theTD == parentTR.cells[i]) { selectedTab = i; }
		}

		if (selectedTab==1) {
			parentTR.cells[0].className = this.id + "_StartTabOn";
		} else {
			parentTR.cells[0].className = this.id + "_StartTabOff";
			parentTR.cells[selectedTab-1].className = this.id + "_TabOffLeft";
		}

		theTD.className = this.id + "_TabOn";
	}
}
FTB_FreeTextBox.prototype.Focus = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		this.designEditor.focus();
		this.UpdateToolbars();
	} else if (this.mode == FTB_MODE_HTML) {
		this.htmlEditor.focus();
	}
	this.hasFocus = true;
}
FTB_FreeTextBox.prototype.SetStyle = function(className) {
	
	// retrieve parent element of the selection
	var parent = this.GetParentElement();
	
	var surround = true;
	var isSpan = (parent && parent.tagName.toLowerCase() == "span");
	
	/*
	// remove class stuff??
	if (isSpan && index == 0 && !/\S/.test(parent.style.cssText)) {
		while (parent.firstChild) {
			parent.parentNode.insertBefore(parent.firstChild, parent);
		}
		parent.parentNode.removeChild(parent);
		this.UpdateToolbars();
		return;
	}
	*/
	
	// if we're already in a SPAN
	if (isSpan) {
		if (parent.childNodes.length == 1) {
			parent.className = className;
			surround = false;
			this.UpdateToolbar();
			return;
		}
	} else {
		
	}

	if (surround) {
		this.SurroundHtml("<span class='" + className + "'>", "</span>");
	}
}

FTB_FreeTextBox.prototype.GetHtml = function() {
	if (this.mode == FTB_MODE_DESIGN)
		this.CopyDesignToHtml();
		
	return this.htmlEditor.value;
}

FTB_FreeTextBox.prototype.SetHtml = function(html) {
	return this.htmlEditor.value = html;
	this.GoToDesignMode();
}

/* START: Button Methods 
-------------------------------- */
FTB_FreeTextBox.prototype.StoreHtml = function() {
	if (this.mode == FTB_MODE_DESIGN)
		this.CopyDesignToHtml();
		
	return true;
}
FTB_FreeTextBox.prototype.DeleteContents = function() {
	if (confirm('Do you want to delete all the HTML and text presently in the editor?')) {	
		this.designEditor.document.body.innerHTML = '';
		this.htmlEditor.value='';
		this.GoToDesignMode();
	}
}

FTB_FreeTextBox.prototype.Cut = function() {
	if (this.mode == FTB_MODE_DESIGN) {

		try {
			this.ExecuteCommand('cut'); 
		} catch (e) {
			alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
		}	
	} else {
		//alert("TODO");
	}
}
FTB_FreeTextBox.prototype.Copy = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		try {
			this.ExecuteCommand('copy');
		} catch (e) {
			alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
		}	
	} else {
		//alert("TODO");
	}
}
FTB_FreeTextBox.prototype.Paste = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		try {
			this.ExecuteCommand('paste'); 
		} catch (e) {
			alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
		}	
	} else {
		//alert("TODO");
	}
}
FTB_FreeTextBox.prototype.SelectAll = function() {
	if (this.mode == FTB_MODE_DESIGN) {		
		this.SelectNodeContents(this.designEditor.document.body);
	}
}
FTB_FreeTextBox.prototype.Print = function() {
	if (this.mode == FTB_MODE_DESIGN) {
		if (FTB_Browser.isIE) {
			this.ExecuteCommand('print'); 
		} else {
			this.designEditor.print();
		}	
	} else {
		printWindow = window.open('','','');
		printWindow.document.open();
		printWindow.document.write("<html><body><pre>" + this.HtmlEncode(this.htmlEditor.value) + "</code></body></html>");
		printWindow.document.close();
		printWindow.document.body.print();
		printWindow.close();
	}
}
FTB_FreeTextBox.prototype.CreateLink = function() {
	if (FTB_Browser.isIE) {
		this.ExecuteCommand('createlink','1',null);
	} else {
		var url = prompt('Enter a URL:', 'http://');
		if ((url != null) && (url != ''))  this.ExecuteCommand('createlink','1',url);
	}
}
FTB_FreeTextBox.prototype.IeSpellCheck = function() {
	if (!FTB_Browser.isIE) {
		alert('IE Spell is not supported in Mozilla');
		return;
	}
	try {
		var tspell = new ActiveXObject('ieSpell.ieSpellExtension');
		tspell.CheckAllLinkedDocuments(window.document);
	} catch (err){
		if (window.confirm('You need ieSpell to use spell check. Would you like to install it?')){
			window.open('http://www.iespell.com/download.php');
		};
	}
}
FTB_FreeTextBox.prototype.NetSpell = function() {
	if (typeof(checkSpellingById) == 'function') {
		checkSpellingById(this.id + '_designEditor');
	} else {
		alert('Netspell libraries not properly linked.');
	}
}
FTB_FreeTextBox.prototype.InsertImage = function() {
	if (FTB_Browser.isIE) {
		this.ExecuteCommand('insertimage','1',null);
	} else {
		var image = prompt('Enter an image location:', 'http://');
		if ((image != null) && (image != ''))  this.ExecuteCommand('insertimage','1',image);	
	}
}
FTB_FreeTextBox.prototype.SaveButton = function() {
	this.StoreHtml();	
	dotNetName = this.id.split('_').join(':');
	__doPostBack(dotNetName,'Save');
}
FTB_FreeTextBox.prototype.InsertImageFromGallery = function() {
	
	url = this.imageGalleryUrl.replace(/\{0\}/g,this.imageGalleryPath);	
	url += "&ftb=" + this.id;

	var gallery = window.open(url,'gallery','width=700,height=600,toolbars=0,resizable=1');
	gallery.focus();	
}


/* --------------------------------
END: Button Methods */

