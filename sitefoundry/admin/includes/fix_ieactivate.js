/*
	- This one is to avoid the activate ActiveX click introduced by Microsoft patch.
   The solutions i found on Microsoft's page or elsewhere don't work for applets or are 
   hard/take-too-much time to implement. 
   Who likes to change the pages to write the whole applet through javascript!? 
   Also tested only on applets, this will work with other objects also.
 - import this file right before the </body> tag with:
 		<script type="text/javascript" src="[your_path]fix_ieactivate.js"></script>
 - you can use this script as you wish, just leave the @author part in.

 @author Cristian Senchiu http://www.senchiu.de
*/ 
if(navigator.appName.indexOf("Microsoft")!=-1){
 var aoTypes 		= new Array("object","applet","embed");
	var iLenTypes	=	aoTypes.length
	try{
  for(var iPosType=0; iPosType<iLenTypes; iPosType++){
  	try{
    	var sCurrType			=	aoTypes[iPosType];
    	var aoReplaceObj	= document.getElementsByTagName(sCurrType);
      if(aoReplaceObj){
        var iLen	=	aoReplaceObj.length;
        for(var iPos=0; iPos<iLen; iPos++){
        	try{
          	//clone old node
          	var oldObj		=	aoReplaceObj[iPos];
            var parentObj	= oldObj.parentNode;
            var newObj		=	oldObj.cloneNode(true);
            //write new node
            document.write(newObj.outerHTML);
            //replace old with new
            var aoReplaceObj2 = document.getElementsByTagName(sCurrType);
            var newObj2				=	aoReplaceObj2[iLen];
            parentObj.replaceChild(newObj2, oldObj);
          }catch(oSecondLoopEx){
          	alert("error in SecondLoop: "+oSecondLoopEx.message);
          }
      	}
      }
    }catch(oFirstLoopEx){
    	alert("error in FirstLoop: "+oFirstLoopEx.message);
    }
  }
	}catch(oGenEx){
 	alert("error: "+oGenEx.message);
 }
}