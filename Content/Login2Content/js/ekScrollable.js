/*
 * ekScrollable - v1.4
 * 
 * Author : Erwan Kuznik
 * http://www.wdev.pro/
 *
 *
 * 1.0 : 02/05/2013
 * 1.4 : 26/11/2013 
 * 
 * Change 1.4 : tactiles et mouse dispo en commun (cas win8 + ecran tactile)
 * Change 1.3 : redimenssionement
 * Change 1.2 : ajout de la prise en charge de la molette
 * Change 1.1 : scroll pour apareil tactiles (deplacement de la cible -> placement du bouton)
 * 
 */

(function( $ )
  {
	$.ekScrollable = function(element, options) 
	  {
		var defaults = 
		  {
			  hauteurScrollable:0,
			  hauteurBoutonMin:0,
			  estScrollable: false,
			  hauteurAffichage:0,
			  initialise:false,
			  degrade:null,
			  enScroll: false,
			  isOn: false,
			  largeurAffichage: 0,
			  hasScrolled:false,
			  multiplicateurMolette:10,
			  premierchargement:false,
			  
			  debug: false,
			  touchActive:false
		  }
		var plugin = this;
        plugin.settings = {};
		
        var $element = $(element), element = element;

		plugin.init = function()
		  {
			plugin.settings = $.extend({}, defaults, options);
			if (plugin.is_touch_device())
			  {plugin.settings.touchActive = true;}
			
			$element.addClass('ekScrollable').wrapInner('<div class="ekScrollableContenu" />').wrapInner('<div class="ekScrollableConteneur" />');
			plugin.settings.zoneConteneur = $element.find('.ekScrollableConteneur');
			if(plugin.settings.hauteurAffichage==0)
			  {plugin.settings.hauteurAffichage = $element.height();}
			if(plugin.settings.largeurAffichage==0)
			  {plugin.settings.largeurAffichage = $element.width();}
			plugin.settings.zoneConteneur.height(plugin.settings.hauteurAffichage);
			plugin.settings.zoneContenu = $element.find('.ekScrollableContenu');
			plugin.settings.zoneConteneur.width(plugin.settings.largeurAffichage/* - 20*/);
			plugin.settings.hauteurContenu = plugin.settings.zoneContenu.outerHeight(true);
			$element.addClass('fondactif');
			
			setTimeout(plugin.initTempo,200);
		  }
		plugin.is_touch_device = function() 
		  {
			return ('ontouchstart' in window) || ('onmsgesturechange' in window);
		  }
		plugin.chekIn = function()
		  {
			//debug('ekScrollable - chekIn');
			if(!plugin.settings.initialise)
			  {
				plugin.initRoll();
				$element.mousewheel(plugin.scrollMolette);
			  }
			if(plugin.settings.estScrollable)
			  {plugin.settings.fondScroll.stop().fadeIn(150);}
			
			plugin.settings.isOn = true;
		  }
		plugin.checkOut = function()
		  {
			//debug('ekScrollable - checkOut');
			if(plugin.settings.estScrollable)
			  {
				if(!plugin.settings.enScroll)
				  {plugin.settings.fondScroll.stop().fadeOut(150);}
			  }
			plugin.settings.isOn = false;
		  }
		
		plugin.recupeSourisCourante = function (e)
		  {
			//debug('ekScrollable - recupeSourisCourante');
			if(e.pageX || e.pageY)
			  {return {x:e.pageX, y:e.pageY};}
			else if(e.originalEvent.clientX || e.originalEvent.clientY)
			  {return {x:e.originalEvent.clientX, y:e.originalEvent.clientY};}
			else if(e.originalEvent.targetTouches[0].pageX)
			  {return {x:e.originalEvent.targetTouches[0].pageX, y:e.originalEvent.targetTouches[0].pageY};}
			else
			  {
				return {
					x: e.originalEvent.pageX,
					y: e.originalEvent.pageY
				};
			  }
		  }
		plugin.debuterDragTouch = function(e)
		  {
			debug('ekScrollable->debuterDragTouch');
			e.preventDefault();
			
			if(!plugin.settings.initialise)
			    {plugin.initRoll();}
			
			plugin.settings.sourisInitial 	= plugin.recupeSourisCourante(e);
			plugin.settings.enScroll 		= true;
			plugin.settings.deplacementMax	= Math.max(0,plugin.settings.hauteurContenu-plugin.settings.hauteurAffichage);
			plugin.settings.posInitial 		= plugin.recupePositionPlan(e);
			
			plugin.settings.fondScroll.stop().fadeIn(250);

			$(document).bind('touchmove',plugin.majPosTouch);
			$(document).bind('touchend',plugin.controlPosTouch);
			
			if(plugin.settings.debug)
			  {
				$(document).bind('mousemove',plugin.majPosTouch);
				$(document).bind('mouseup',plugin.controlPosTouch);
			  }
			
			$('body').addClass('isDrag');
		  }
		plugin.majPosTouch = function(e)
		  {
			//debug('ekScrollable - majPosTouch');
			plugin.settings.sourisCourante = plugin.recupeSourisCourante(e);
			var decalage = plugin.settings.sourisCourante.y - plugin.settings.sourisInitial.y;
			
			var cpt = Math.max(0,plugin.settings.posInitial.y-decalage);
			var ny = Math.min(plugin.settings.deplacementMax,cpt);
			//debug(decalage+" - "+ny);
			plugin.settings.zoneConteneur.scrollTop(ny);
			
			var ratio = plugin.recupRatioReverse();
			plugin.btnVers(ratio);
		  }
		plugin.recupRatioReverse = function()
		  {
			//debug('ekScrollable - recupRatioReverse');
			var py = parseInt(plugin.settings.zoneConteneur.scrollTop());
			return Math.round(100 * py / plugin.settings.deplacementMax ) / 100;
		  }
		plugin.controlPosTouch = function()
		  {
			//debug('ekScrollable - controlPosTouch');
			plugin.settings.enScroll =  false;
			
			$(document).unbind('touchmove');
			$(document).unbind('touchend');
			
			if(plugin.settings.debug)
			  {
				$(document).unbind('mousemove');
				$(document).unbind('mouseup');
			  }
			plugin.settings.fondScroll.stop().fadeOut(250);
			
			var r = plugin.recupRatioReverse();
			if(r<0)
			  {plugin.btnVers(0);}
			else if(r>1)
			  {plugin.btnVers(1);}
		  }
		plugin.recupePositionPlan = function (e)
		  {return {x:plugin.settings.zoneConteneur.scrollLeft(), y:plugin.settings.zoneConteneur.scrollTop()};}
		plugin.initTempo = function()
		  {
			//debug('ekScrollable - initTempo');
			if(!plugin.settings.premierchargement)
			  {plugin.settings.fondScroll = $('<div class="fondScroll" style="display: none;" />').appendTo($element);}
			plugin.settings.fondScroll.height(plugin.settings.hauteurAffichage);
			
			if(plugin.settings.largeurAffichage==0)
			  {plugin.settings.largeurAffichage = $element.width();}
			var l20 = plugin.settings.largeurAffichage - 20;
			var h40 = plugin.settings.hauteurAffichage - 40;
			//debug('l20 - '+l20);
			plugin.settings.zoneConteneur.width(l20);
			
			if(!plugin.settings.premierchargement)
			  {
				plugin.settings.degrade = $('<div class="fondufin" style="width:'+l20+'px; margin-top:'+h40+'px;" />');
				plugin.attacherEvements();
			  }
			plugin.settings.premierchargement = true;
		  }
		plugin.attacherEvements = function()
		  {
			//debug('ekScrollable - attacherEvements');
			$('<div class="degrade" />').appendTo(plugin.settings.degrade);
			plugin.settings.degrade.appendTo($element);
			
			$element.hover(plugin.chekIn, plugin.checkOut);
			plugin.settings.degrade.addClass('desactive');
			
			if (plugin.settings.touchActive)
			  {$element.bind('touchstart',plugin.debuterDragTouch);}
			if(plugin.settings.debug)
			  {$element.bind('mousedown',plugin.debuterDragTouch);}
		  }
		
		
		
		
		plugin.redim = function(hauteur)
		  {
			var pos = 0;
			if(plugin.settings.initialise)
			  {pos = plugin.recupRatio();}
			plugin.settings.hauteurAffichage = hauteur;
			$element.height(plugin.settings.hauteurAffichage-15);
			plugin.settings.zoneConteneur.height(plugin.settings.hauteurAffichage);
			var h40 = plugin.settings.hauteurAffichage - 40;
			
			if(plugin.settings.premierchargement)
			  {
				plugin.settings.degrade.css('margin-top',h40);
				plugin.settings.fondScroll.height(plugin.settings.hauteurAffichage);
				plugin.settings.degrade.height(plugin.settings.hauteurAffichage);
			  }
			if(plugin.settings.initialise)
			  {
				plugin.settings.hauteurContenu = plugin.settings.zoneContenu.outerHeight(true);
				plugin.settings.pctH = plugin.settings.hauteurAffichage / plugin.settings.hauteurContenu;
				plugin.settings.hauteurBoutonScroll = Math.max(plugin.settings.hauteurAffichage*plugin.settings.pctH,plugin.settings.hauteurBoutonMin);
				plugin.settings.hauteurScrollable = plugin.settings.hauteurContenu - plugin.settings.hauteurAffichage;
				plugin.settings.deplacementSlider = plugin.settings.fondScroll.height() - plugin.settings.hauteurBoutonScroll;
				plugin.settings.boutonScroll.css({height:plugin.settings.hauteurBoutonScroll});
				
				var seraScrollable = (plugin.settings.hauteurAffichage<plugin.settings.hauteurContenu);
				if(!plugin.settings.estScrollable && seraScrollable)
				  {plugin.settings.degrade.stop().fadeIn(250);}
				else if(plugin.settings.estScrollable && !seraScrollable)
				  {plugin.settings.degrade.stop().fadeOut(250);}
				plugin.settings.estScrollable = seraScrollable;
				//debug(plugin.recupRatio());
				plugin.scrollEtBtnVers(pos);
			  }
			plugin.settings.hasScrolled = false;
		  }
		
		
		
		/**** SOURIS ****/
		plugin.scrollMolette = function(e, delta, deltaX, deltaY)
		  {
			//debug('ekScrollable - scrollMolette - '+delta+', '+deltaX+', '+deltaY);
			if(isNaN(deltaY))
			  {deltaY = delta;}
			if(!plugin.settings.hasScrolled)
			  {
				plugin.settings.deplacementMax	= Math.max(0,plugin.settings.hauteurContenu-plugin.settings.hauteurAffichage);
				plugin.settings.hasScrolled = true;
			  }
			plugin.settings.posInitial	= plugin.recupePositionPlan(e);
			
			var cpt = Math.max(0,plugin.settings.posInitial.y-deltaY*plugin.settings.multiplicateurMolette);
			var ny = Math.min(plugin.settings.deplacementMax,cpt);
			plugin.settings.zoneConteneur.scrollTop(ny);
			
			var ratio = plugin.recupRatioReverse();
			plugin.btnVers(ratio);
			
			return false;
		  }
		
		plugin.initRoll = function()
		  {
			//debug('ekScrollable - initRoll');
			plugin.settings.hauteurContenu = plugin.settings.zoneContenu.outerHeight(true);
			plugin.settings.estScrollable = (plugin.settings.hauteurAffichage<plugin.settings.hauteurContenu);
			
			plugin.settings.boutonScrollCont = $('<div class="btnScrollCont" />').appendTo(plugin.settings.fondScroll);
			plugin.settings.boutonScroll = $('<div class="btnScroll" />').appendTo(plugin.settings.boutonScrollCont);
			
			plugin.settings.fondScroll.click(function(e) {plugin.clickPos(e);});
			plugin.settings.pctH = plugin.settings.hauteurAffichage / plugin.settings.hauteurContenu;
			plugin.settings.hauteurBoutonScroll = Math.max(plugin.settings.hauteurAffichage*plugin.settings.pctH,plugin.settings.hauteurBoutonMin);
			
			plugin.settings.hauteurScrollable = plugin.settings.hauteurContenu - plugin.settings.hauteurAffichage;
			plugin.settings.deplacementSlider = plugin.settings.fondScroll.height() - plugin.settings.hauteurBoutonScroll;
			
			plugin.settings.boutonScroll.css({height:plugin.settings.hauteurBoutonScroll});
			
			if(!plugin.settings.estScrollable)
			  {plugin.settings.degrade.stop().fadeOut(250);}
			
			if (!plugin.settings.touchActive)
			  {
				plugin.settings.boutonScrollCont.draggable({
					axis:"y", 
					containment:plugin.settings.fondScroll, 
					drag: plugin.majPos, 
					stop: plugin.controlPos, 
					start: function() {plugin.settings.enScroll = true;}
				  });
			  }
			else
			  {plugin.settings.boutonScrollCont.css('position','relative');}
			plugin.scrollEtBtnVers(0);
			plugin.settings.initialise = true;
		  }
		plugin.clickPos = function(e)
		  {
			//debug('ekScrollable - clickPos');
			var nPosY = Math.min(e.pageY - plugin.settings.fondScroll.offset().top,plugin.settings.fondScroll.height()-plugin.settings.boutonScroll.height());
			plugin.settings.boutonScrollCont.css('top',nPosY);
			plugin.majPos();
		  }
		
		
		
		plugin.controlPos = function()
		  {
			//debug('ekScrollable - controlPos');
			plugin.settings.enScroll =  false;
			var r = plugin.recupRatio();
			if(r<0)
			  {plugin.scrollEtBtnVers(0);}
			else if(r>1)
			  {plugin.scrollEtBtnVers(1);}
			if(!plugin.settings.isOn)
			  {plugin.checkOut();}
		  }
		
		
		
		
		
		
		
		plugin.recupRatio = function()
		  {
			//debug('ekScrollable - recupRatio');
			var py = parseInt(plugin.settings.boutonScrollCont.css('top'));
			return Math.round(100 * py / plugin.settings.deplacementSlider ) / 100;
		  }
		plugin.majPos = function()
		  {
			//debug('ekScrollable - majPos');
			var ratio = plugin.recupRatio();
			plugin.scrollVers(ratio);
		  }
		plugin.btnVers = function(ratio)
		  {
			//debug('ekScrollable - btnVers');
			plugin.settings.boutonScrollCont.css('top',ratio*plugin.settings.deplacementSlider);
			//debug(ratio*plugin.settings.deplacementSlider+" - "+ratio+"*"+plugin.settings.deplacementSlider);
		  }
		plugin.scrollEtBtnVers = function(ratio)
		  {
			//debug('ekScrollable - scrollEtBtnVers');
			plugin.settings.boutonScrollCont.css('top',ratio*plugin.settings.deplacementSlider);
			plugin.scrollVers(ratio);
		  }
		plugin.scrollVers = function(ratio)
		  {
			//debug('ekScrollable - scrollVers');
			plugin.settings.zoneConteneur.scrollTop(ratio*plugin.settings.hauteurScrollable);
		  }
		plugin.init();
	  };

	$.fn.ekScrollable = function(options) 
	  {
		return this.each(function() 
		  {
			if (undefined == $(this).data('ekScrollable')) 
			  {
				var plugin = new $.ekScrollable(this, options);
				$(this).data('ekScrollable', plugin);
			  }
		  });
	  }
  

})(jQuery);

