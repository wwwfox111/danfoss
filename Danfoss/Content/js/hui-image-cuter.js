"use strict";

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

/*
hui 图片剪裁组件
作者 : 深海  5213606@qq.com
官网 : http://www.hcoder.net/hui
*/
!function (t, i) {
	"object" == (typeof exports === "undefined" ? "undefined" : _typeof(exports)) && "undefined" != typeof module ? module.exports = i() : "function" == typeof define && define.amd ? define(i) : t.Cropper = i();
}(undefined, function () {
	"use strict";
	var t = "undefined" != typeof window ? window : {},
	    i = "cropper",
	    e = i + "-crop",
	    a = i + "-disabled",
	    n = i + "-hidden",
	    o = i + "-hide",
	    h = i + "-modal",
	    r = i + "-move",
	    s = "action",
	    c = "preview",
	    l = "crop",
	    d = "cropend",
	    p = "cropmove",
	    m = "cropstart",
	    u = "load",
	    g = t.PointerEvent ? "pointerdown" : "touchstart mousedown",
	    f = t.PointerEvent ? "pointermove" : "touchmove mousemove",
	    v = t.PointerEvent ? "pointerup pointercancel" : "touchend touchcancel mouseup",
	    w = "wheel mousewheel DOMMouseScroll",
	    x = /^(e|w|s|n|se|sw|ne|nw|all|crop|move|zoom)$/,
	    b = /^data:/,
	    y = /^data:image\/jpeg;base64,/,
	    C = /^(img|canvas)$/i,
	    M = { viewMode: 0, dragMode: "crop", aspectRatio: NaN, data: null, preview: "", responsive: !0, restore: !0, checkCrossOrigin: !0, checkOrientation: !0, modal: !0, guides: !0, center: !0, highlight: !0, background: !0, autoCrop: !0, autoCropArea: .8, movable: !0, rotatable: !0, scalable: !0, zoomable: !0, zoomOnTouch: !0, zoomOnWheel: !0, wheelZoomRatio: .1, cropBoxMovable: !0, cropBoxResizable: !0, toggleDragModeOnDblclick: !0, minCanvasWidth: 0, minCanvasHeight: 0, minCropBoxWidth: 0, minCropBoxHeight: 0, minContainerWidth: 200, minContainerHeight: 100, ready: null, cropstart: null, cropmove: null, cropend: null, crop: null, zoom: null },
	    D = "function" == typeof Symbol && "symbol" == _typeof(Symbol.iterator) ? function (t) {
		return typeof t === "undefined" ? "undefined" : _typeof(t);
	} : function (t) {
		return t && "function" == typeof Symbol && t.constructor === Symbol && t !== Symbol.prototype ? "symbol" : typeof t === "undefined" ? "undefined" : _typeof(t);
	},
	    B = function B(t, i) {
		if (!(t instanceof i)) throw new TypeError("Cannot call a class as a function");
	},
	    k = function () {
		function t(t, i) {
			for (var e = 0; e < i.length; e++) {
				var a = i[e];a.enumerable = a.enumerable || !1, a.configurable = !0, "value" in a && (a.writable = !0), Object.defineProperty(t, a.key, a);
			}
		}return function (i, e, a) {
			return e && t(i.prototype, e), a && t(i, a), i;
		};
	}(),
	    E = function E(t) {
		if (Array.isArray(t)) {
			for (var i = 0, e = Array(t.length); i < t.length; i++) {
				e[i] = t[i];
			}return e;
		}return Array.from(t);
	},
	    T = Number.isNaN || t.isNaN;function W(t) {
		return "number" == typeof t && !T(t);
	}function N(t) {
		return void 0 === t;
	}function H(t) {
		return "object" === (void 0 === t ? "undefined" : D(t)) && null !== t;
	}var L = Object.prototype.hasOwnProperty;function Y(t) {
		if (!H(t)) return !1;try {
			var i = t.constructor,
			    e = i.prototype;return i && e && L.call(e, "isPrototypeOf");
		} catch (t) {
			return !1;
		}
	}function X(t) {
		return "function" == typeof t;
	}function O(t, i) {
		if (t && X(i)) if (Array.isArray(t) || W(t.length)) {
			var e = t.length,
			    a = void 0;for (a = 0; a < e && !1 !== i.call(t, t[a], a, t); a += 1) {}
		} else H(t) && Object.keys(t).forEach(function (e) {
			i.call(t, t[e], e, t);
		});return t;
	}function S(t) {
		for (var i = arguments.length, e = Array(i > 1 ? i - 1 : 0), a = 1; a < i; a++) {
			e[a - 1] = arguments[a];
		}if (H(t) && e.length > 0) {
			if (Object.assign) return Object.assign.apply(Object, [t].concat(e));e.forEach(function (i) {
				H(i) && Object.keys(i).forEach(function (e) {
					t[e] = i[e];
				});
			});
		}return t;
	}function z(t, i) {
		for (var e = arguments.length, a = Array(e > 2 ? e - 2 : 0), n = 2; n < e; n++) {
			a[n - 2] = arguments[n];
		}return function () {
			for (var e = arguments.length, n = Array(e), o = 0; o < e; o++) {
				n[o] = arguments[o];
			}return t.apply(i, a.concat(n));
		};
	}var R = /\.\d*(?:0|9){12}\d*$/i;function A(t) {
		var i = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : 1e11;return R.test(t) ? Math.round(t * i) / i : t;
	}var I = /^(width|height|left|top|marginLeft|marginTop)$/;function U(t, i) {
		var e = t.style;O(i, function (t, i) {
			I.test(i) && W(t) && (t += "px"), e[i] = t;
		});
	}function j(t, i) {
		if (i) if (W(t.length)) O(t, function (t) {
			j(t, i);
		});else if (t.classList) t.classList.add(i);else {
			var e = t.className.trim();e ? e.indexOf(i) < 0 && (t.className = e + " " + i) : t.className = i;
		}
	}function P(t, i) {
		i && (W(t.length) ? O(t, function (t) {
			P(t, i);
		}) : t.classList ? t.classList.remove(i) : t.className.indexOf(i) >= 0 && (t.className = t.className.replace(i, "")));
	}function q(t, i, e) {
		i && (W(t.length) ? O(t, function (t) {
			q(t, i, e);
		}) : e ? j(t, i) : P(t, i));
	}var $ = /([a-z\d])([A-Z])/g;function Q(t) {
		return t.replace($, "$1-$2").toLowerCase();
	}function Z(t, i) {
		return H(t[i]) ? t[i] : t.dataset ? t.dataset[i] : t.getAttribute("data-" + Q(i));
	}function F(t, i, e) {
		H(e) ? t[i] = e : t.dataset ? t.dataset[i] = e : t.setAttribute("data-" + Q(i), e);
	}function K(t, i) {
		if (H(t[i])) try {
			delete t[i];
		} catch (e) {
			t[i] = null;
		} else if (t.dataset) try {
			delete t.dataset[i];
		} catch (e) {
			t.dataset[i] = null;
		} else t.removeAttribute("data-" + Q(i));
	}var V = /\s+/;function G(t, i, e) {
		var a = arguments.length > 3 && void 0 !== arguments[3] ? arguments[3] : {};if (X(e)) {
			var n = i.trim().split(V);n.length > 1 ? O(n, function (i) {
				G(t, i, e, a);
			}) : t.removeEventListener ? t.removeEventListener(i, e, a) : t.detachEvent && t.detachEvent("on" + i, e);
		}
	}function J(t, i, _e) {
		var a = arguments.length > 3 && void 0 !== arguments[3] ? arguments[3] : {};if (X(_e)) {
			var n = i.trim().split(V);if (n.length > 1) O(n, function (i) {
				J(t, i, _e, a);
			});else {
				if (a.once) {
					var o = _e;_e = function e() {
						for (var n = arguments.length, h = Array(n), r = 0; r < n; r++) {
							h[r] = arguments[r];
						}return G(t, i, _e, a), o.apply(t, h);
					};
				}t.addEventListener ? t.addEventListener(i, _e, a) : t.attachEvent && t.attachEvent("on" + i, _e);
			}
		}
	}function _(t, i, e) {
		if (t.dispatchEvent) {
			var a = void 0;return X(Event) && X(CustomEvent) ? a = N(e) ? new Event(i, { bubbles: !0, cancelable: !0 }) : new CustomEvent(i, { detail: e, bubbles: !0, cancelable: !0 }) : N(e) ? (a = document.createEvent("Event")).initEvent(i, !0, !0) : (a = document.createEvent("CustomEvent")).initCustomEvent(i, !0, !0, e), t.dispatchEvent(a);
		}return !t.fireEvent || t.fireEvent("on" + i);
	}function tt(t) {
		var i = document.documentElement,
		    e = t.getBoundingClientRect();return { left: e.left + ((window.scrollX || i && i.scrollLeft || 0) - (i && i.clientLeft || 0)), top: e.top + ((window.scrollY || i && i.scrollTop || 0) - (i && i.clientTop || 0)) };
	}var it = t.location,
	    et = /^(https?:)\/\/([^:/?#]+):?(\d*)/i;function at(t) {
		var i = t.match(et);return i && (i[1] !== it.protocol || i[2] !== it.hostname || i[3] !== it.port);
	}function nt(t) {
		var i = "timestamp=" + new Date().getTime();return t + (-1 === t.indexOf("?") ? "?" : "&") + i;
	}function ot(t) {
		var i = t.rotate,
		    e = t.scaleX,
		    a = t.scaleY,
		    n = t.translateX,
		    o = t.translateY,
		    h = [];W(n) && 0 !== n && h.push("translateX(" + n + "px)"), W(o) && 0 !== o && h.push("translateY(" + o + "px)"), W(i) && 0 !== i && h.push("rotate(" + i + "deg)"), W(e) && 1 !== e && h.push("scaleX(" + e + ")"), W(a) && 1 !== a && h.push("scaleY(" + a + ")");var r = h.length ? h.join(" ") : "none";return { WebkitTransform: r, msTransform: r, transform: r };
	}var ht = t.navigator,
	    rt = ht && /(Macintosh|iPhone|iPod|iPad).*AppleWebKit/i.test(ht.userAgent);function st(t, i) {
		var e = t.pageX,
		    a = t.pageY,
		    n = { endX: e, endY: a };return i ? n : S({ startX: e, startY: a }, n);
	}var ct = Number.isFinite || t.isFinite;function lt(t) {
		var i = t.aspectRatio,
		    e = t.height,
		    a = t.width,
		    n = function n(t) {
			return ct(t) && t > 0;
		};return n(a) && n(e) ? e * i > a ? e = a / i : a = e * i : n(a) ? e = a / i : n(e) && (a = e * i), { width: a, height: e };
	}var dt = String.fromCharCode;var pt = /^data:.*,/;function mt(t) {
		var i = new DataView(t),
		    e = void 0,
		    a = void 0,
		    n = void 0,
		    o = void 0;if (255 === i.getUint8(0) && 216 === i.getUint8(1)) for (var h = i.byteLength, r = 2; r < h;) {
			if (255 === i.getUint8(r) && 225 === i.getUint8(r + 1)) {
				n = r;break;
			}r += 1;
		}if (n) {
			var s = n + 10;if ("Exif" === function (t, i, e) {
				var a = "",
				    n = void 0;for (e += i, n = i; n < e; n += 1) {
					a += dt(t.getUint8(n));
				}return a;
			}(i, n + 4, 4)) {
				var c = i.getUint16(s);if (((a = 18761 === c) || 19789 === c) && 42 === i.getUint16(s + 2, a)) {
					var l = i.getUint32(s + 4, a);l >= 8 && (o = s + l);
				}
			}
		}if (o) {
			var d = i.getUint16(o, a),
			    p = void 0,
			    m = void 0;for (m = 0; m < d; m += 1) {
				if (p = o + 12 * m + 2, 274 === i.getUint16(p, a)) {
					p += 8, e = i.getUint16(p, a), i.setUint16(p, 1, a);break;
				}
			}
		}return e;
	}var ut = { render: function render() {
			this.initContainer(), this.initCanvas(), this.initCropBox(), this.renderCanvas(), this.cropped && this.renderCropBox();
		}, initContainer: function initContainer() {
			var t = this.element,
			    i = this.options,
			    e = this.container,
			    a = this.cropper;j(a, n), P(t, n);var o = { width: Math.max(e.offsetWidth, Number(i.minContainerWidth) || 200), height: Math.max(e.offsetHeight, Number(i.minContainerHeight) || 100) };this.containerData = o, U(a, { width: o.width, height: o.height }), j(t, n), P(a, n);
		}, initCanvas: function initCanvas() {
			var t = this.containerData,
			    i = this.imageData,
			    e = this.options.viewMode,
			    a = Math.abs(i.rotate) % 180 == 90,
			    n = a ? i.naturalHeight : i.naturalWidth,
			    o = a ? i.naturalWidth : i.naturalHeight,
			    h = n / o,
			    r = t.width,
			    s = t.height;t.height * h > t.width ? 3 === e ? r = t.height * h : s = t.width / h : 3 === e ? s = t.width / h : r = t.height * h;var c = { aspectRatio: h, naturalWidth: n, naturalHeight: o, width: r, height: s };c.left = (t.width - r) / 2, c.top = (t.height - s) / 2, c.oldLeft = c.left, c.oldTop = c.top, this.canvasData = c, this.limited = 1 === e || 2 === e, this.limitCanvas(!0, !0), this.initialImageData = S({}, i), this.initialCanvasData = S({}, c);
		}, limitCanvas: function limitCanvas(t, i) {
			var e = this.options,
			    a = this.containerData,
			    n = this.canvasData,
			    o = this.cropBoxData,
			    h = e.viewMode,
			    r = n.aspectRatio,
			    s = this.cropped && o;if (t) {
				var c = Number(e.minCanvasWidth) || 0,
				    l = Number(e.minCanvasHeight) || 0;h > 1 ? (c = Math.max(c, a.width), l = Math.max(l, a.height), 3 === h && (l * r > c ? c = l * r : l = c / r)) : h > 0 && (c ? c = Math.max(c, s ? o.width : 0) : l ? l = Math.max(l, s ? o.height : 0) : s && (c = o.width, (l = o.height) * r > c ? c = l * r : l = c / r));var d = lt({ aspectRatio: r, width: c, height: l });c = d.width, l = d.height, n.minWidth = c, n.minHeight = l, n.maxWidth = 1 / 0, n.maxHeight = 1 / 0;
			}if (i) if (h) {
				var p = a.width - n.width,
				    m = a.height - n.height;n.minLeft = Math.min(0, p), n.minTop = Math.min(0, m), n.maxLeft = Math.max(0, p), n.maxTop = Math.max(0, m), s && this.limited && (n.minLeft = Math.min(o.left, o.left + (o.width - n.width)), n.minTop = Math.min(o.top, o.top + (o.height - n.height)), n.maxLeft = o.left, n.maxTop = o.top, 2 === h && (n.width >= a.width && (n.minLeft = Math.min(0, p), n.maxLeft = Math.max(0, p)), n.height >= a.height && (n.minTop = Math.min(0, m), n.maxTop = Math.max(0, m))));
			} else n.minLeft = -n.width, n.minTop = -n.height, n.maxLeft = a.width, n.maxTop = a.height;
		}, renderCanvas: function renderCanvas(t, i) {
			var e = this.canvasData,
			    a = this.imageData;if (i) {
				var n = function (t) {
					var i = t.width,
					    e = t.height,
					    a = t.degree;if (90 == (a = Math.abs(a) % 180)) return { width: e, height: i };var n = a % 90 * Math.PI / 180,
					    o = Math.sin(n),
					    h = Math.cos(n),
					    r = i * h + e * o,
					    s = i * o + e * h;return a > 90 ? { width: s, height: r } : { width: r, height: s };
				}({ width: a.naturalWidth * Math.abs(a.scaleX || 1), height: a.naturalHeight * Math.abs(a.scaleY || 1), degree: a.rotate || 0 }),
				    o = n.width,
				    h = n.height,
				    r = e.width * (o / e.naturalWidth),
				    s = e.height * (h / e.naturalHeight);e.left -= (r - e.width) / 2, e.top -= (s - e.height) / 2, e.width = r, e.height = s, e.aspectRatio = o / h, e.naturalWidth = o, e.naturalHeight = h, this.limitCanvas(!0, !1);
			}(e.width > e.maxWidth || e.width < e.minWidth) && (e.left = e.oldLeft), (e.height > e.maxHeight || e.height < e.minHeight) && (e.top = e.oldTop), e.width = Math.min(Math.max(e.width, e.minWidth), e.maxWidth), e.height = Math.min(Math.max(e.height, e.minHeight), e.maxHeight), this.limitCanvas(!1, !0), e.left = Math.min(Math.max(e.left, e.minLeft), e.maxLeft), e.top = Math.min(Math.max(e.top, e.minTop), e.maxTop), e.oldLeft = e.left, e.oldTop = e.top, U(this.canvas, S({ width: e.width, height: e.height }, ot({ translateX: e.left, translateY: e.top }))), this.renderImage(t), this.cropped && this.limited && this.limitCropBox(!0, !0);
		}, renderImage: function renderImage(t) {
			var i = this.canvasData,
			    e = this.imageData,
			    a = e.naturalWidth * (i.width / i.naturalWidth),
			    n = e.naturalHeight * (i.height / i.naturalHeight);S(e, { width: a, height: n, left: (i.width - a) / 2, top: (i.height - n) / 2 }), U(this.image, S({ width: e.width, height: e.height }, ot(S({ translateX: e.left, translateY: e.top }, e)))), t && this.output();
		}, initCropBox: function initCropBox() {
			var t = this.options,
			    i = this.canvasData,
			    e = t.aspectRatio,
			    a = Number(t.autoCropArea) || .8,
			    n = { width: i.width, height: i.height };e && (i.height * e > i.width ? n.height = n.width / e : n.width = n.height * e), this.cropBoxData = n, this.limitCropBox(!0, !0), n.width = Math.min(Math.max(n.width, n.minWidth), n.maxWidth), n.height = Math.min(Math.max(n.height, n.minHeight), n.maxHeight), n.width = Math.max(n.minWidth, n.width * a), n.height = Math.max(n.minHeight, n.height * a), n.left = i.left + (i.width - n.width) / 2, n.top = i.top + (i.height - n.height) / 2, n.oldLeft = n.left, n.oldTop = n.top, this.initialCropBoxData = S({}, n);
		}, limitCropBox: function limitCropBox(t, i) {
			var e = this.options,
			    a = this.containerData,
			    n = this.canvasData,
			    o = this.cropBoxData,
			    h = this.limited,
			    r = e.aspectRatio;if (t) {
				var s = Number(e.minCropBoxWidth) || 0,
				    c = Number(e.minCropBoxHeight) || 0,
				    l = Math.min(a.width, h ? n.width : a.width),
				    d = Math.min(a.height, h ? n.height : a.height);s = Math.min(s, a.width), c = Math.min(c, a.height), r && (s && c ? c * r > s ? c = s / r : s = c * r : s ? c = s / r : c && (s = c * r), d * r > l ? d = l / r : l = d * r), o.minWidth = Math.min(s, l), o.minHeight = Math.min(c, d), o.maxWidth = l, o.maxHeight = d;
			}i && (h ? (o.minLeft = Math.max(0, n.left), o.minTop = Math.max(0, n.top), o.maxLeft = Math.min(a.width, n.left + n.width) - o.width, o.maxTop = Math.min(a.height, n.top + n.height) - o.height) : (o.minLeft = 0, o.minTop = 0, o.maxLeft = a.width - o.width, o.maxTop = a.height - o.height));
		}, renderCropBox: function renderCropBox() {
			var t = this.options,
			    i = this.containerData,
			    e = this.cropBoxData;(e.width > e.maxWidth || e.width < e.minWidth) && (e.left = e.oldLeft), (e.height > e.maxHeight || e.height < e.minHeight) && (e.top = e.oldTop), e.width = Math.min(Math.max(e.width, e.minWidth), e.maxWidth), e.height = Math.min(Math.max(e.height, e.minHeight), e.maxHeight), this.limitCropBox(!1, !0), e.left = Math.min(Math.max(e.left, e.minLeft), e.maxLeft), e.top = Math.min(Math.max(e.top, e.minTop), e.maxTop), e.oldLeft = e.left, e.oldTop = e.top, t.movable && t.cropBoxMovable && F(this.face, s, e.width >= i.width && e.height >= i.height ? "move" : "all"), U(this.cropBox, S({ width: e.width, height: e.height }, ot({ translateX: e.left, translateY: e.top }))), this.cropped && this.limited && this.limitCanvas(!0, !0), this.disabled || this.output();
		}, output: function output() {
			this.preview(), this.complete && _(this.element, l, this.getData());
		} },
	    gt = { initPreview: function initPreview() {
			var t = this.crossOrigin,
			    i = this.options.preview,
			    e = t ? this.crossOriginUrl : this.url,
			    a = document.createElement("img");if (t && (a.crossOrigin = t), a.src = e, this.viewBox.appendChild(a), this.image2 = a, i) {
				var n = i.querySelector ? [i] : document.querySelectorAll(i);this.previews = n, O(n, function (i) {
					var a = document.createElement("img");F(i, c, { width: i.offsetWidth, height: i.offsetHeight, html: i.innerHTML }), t && (a.crossOrigin = t), a.src = e, a.style.cssText = 'display:block;width:100%;height:auto;min-width:0!important;min-height:0!important;max-width:none!important;max-height:none!important;image-orientation:0deg!important;"', function (t) {
						for (; t.firstChild;) {
							t.removeChild(t.firstChild);
						}
					}(i), i.appendChild(a);
				});
			}
		}, resetPreview: function resetPreview() {
			O(this.previews, function (t) {
				var i = Z(t, c);U(t, { width: i.width, height: i.height }), t.innerHTML = i.html, K(t, c);
			});
		}, preview: function preview() {
			var t = this.imageData,
			    i = this.canvasData,
			    e = this.cropBoxData,
			    a = e.width,
			    n = e.height,
			    o = t.width,
			    h = t.height,
			    r = e.left - i.left - t.left,
			    s = e.top - i.top - t.top;this.cropped && !this.disabled && (U(this.image2, S({ width: o, height: h }, ot(S({ translateX: -r, translateY: -s }, t)))), O(this.previews, function (i) {
				var e = Z(i, c),
				    l = e.width,
				    d = e.height,
				    p = l,
				    m = d,
				    u = 1;a && (m = n * (u = l / a)), n && m > d && (p = a * (u = d / n), m = d), U(i, { width: p, height: m }), U(i.getElementsByTagName("img")[0], S({ width: o * u, height: h * u }, ot(S({ translateX: -r * u, translateY: -s * u }, t))));
			}));
		} },
	    ft = { bind: function bind() {
			var t = this.element,
			    i = this.options,
			    e = this.cropper;X(i.cropstart) && J(t, m, i.cropstart), X(i.cropmove) && J(t, p, i.cropmove), X(i.cropend) && J(t, d, i.cropend), X(i.crop) && J(t, l, i.crop), X(i.zoom) && J(t, "zoom", i.zoom), J(e, g, this.onCropStart = z(this.cropStart, this)), i.zoomable && i.zoomOnWheel && J(e, w, this.onWheel = z(this.wheel, this)), i.toggleDragModeOnDblclick && J(e, "dblclick", this.onDblclick = z(this.dblclick, this)), J(t.ownerDocument, f, this.onCropMove = z(this.cropMove, this)), J(t.ownerDocument, v, this.onCropEnd = z(this.cropEnd, this)), i.responsive && J(window, "resize", this.onResize = z(this.resize, this));
		}, unbind: function unbind() {
			var t = this.element,
			    i = this.options,
			    e = this.cropper;X(i.cropstart) && G(t, m, i.cropstart), X(i.cropmove) && G(t, p, i.cropmove), X(i.cropend) && G(t, d, i.cropend), X(i.crop) && G(t, l, i.crop), X(i.zoom) && G(t, "zoom", i.zoom), G(e, g, this.onCropStart), i.zoomable && i.zoomOnWheel && G(e, w, this.onWheel), i.toggleDragModeOnDblclick && G(e, "dblclick", this.onDblclick), G(t.ownerDocument, f, this.onCropMove), G(t.ownerDocument, v, this.onCropEnd), i.responsive && G(window, "resize", this.onResize);
		} },
	    vt = { resize: function resize() {
			var t = this.options,
			    i = this.container,
			    e = this.containerData,
			    a = Number(t.minContainerWidth) || 200,
			    n = Number(t.minContainerHeight) || 100;if (!(this.disabled || e.width <= a || e.height <= n)) {
				var o = i.offsetWidth / e.width;if (1 !== o || i.offsetHeight !== e.height) {
					var h = void 0,
					    r = void 0;t.restore && (h = this.getCanvasData(), r = this.getCropBoxData()), this.render(), t.restore && (this.setCanvasData(O(h, function (t, i) {
						h[i] = t * o;
					})), this.setCropBoxData(O(r, function (t, i) {
						r[i] = t * o;
					})));
				}
			}
		}, dblclick: function dblclick() {
			if (!this.disabled && "none" !== this.options.dragMode) {
				this.setDragMode((t = this.dragBox, i = e, (t.classList ? t.classList.contains(i) : t.className.indexOf(i) > -1) ? "move" : "crop"));var t, i;
			}
		}, wheel: function wheel(t) {
			var i = this,
			    e = Number(this.options.wheelZoomRatio) || .1,
			    a = 1;this.disabled || (t.preventDefault(), this.wheeling || (this.wheeling = !0, setTimeout(function () {
				i.wheeling = !1;
			}, 50), t.deltaY ? a = t.deltaY > 0 ? 1 : -1 : t.wheelDelta ? a = -t.wheelDelta / 120 : t.detail && (a = t.detail > 0 ? 1 : -1), this.zoom(-a * e, t)));
		}, cropStart: function cropStart(t) {
			if (!this.disabled) {
				var i = this.options,
				    e = this.pointers,
				    a = void 0;t.changedTouches ? O(t.changedTouches, function (t) {
					e[t.identifier] = st(t);
				}) : e[t.pointerId || 0] = st(t), a = Object.keys(e).length > 1 && i.zoomable && i.zoomOnTouch ? "zoom" : Z(t.target, s), x.test(a) && !1 !== _(this.element, m, { originalEvent: t, action: a }) && (t.preventDefault(), this.action = a, this.cropping = !1, "crop" === a && (this.cropping = !0, j(this.dragBox, h)));
			}
		}, cropMove: function cropMove(t) {
			var i = this.action;if (!this.disabled && i) {
				var e = this.pointers;t.preventDefault(), !1 !== _(this.element, p, { originalEvent: t, action: i }) && (t.changedTouches ? O(t.changedTouches, function (t) {
					S(e[t.identifier], st(t, !0));
				}) : S(e[t.pointerId || 0], st(t, !0)), this.change(t));
			}
		}, cropEnd: function cropEnd(t) {
			if (!this.disabled) {
				var i = this.action,
				    e = this.pointers;t.changedTouches ? O(t.changedTouches, function (t) {
					delete e[t.identifier];
				}) : delete e[t.pointerId || 0], i && (t.preventDefault(), Object.keys(e).length || (this.action = ""), this.cropping && (this.cropping = !1, q(this.dragBox, h, this.cropped && this.options.modal)), _(this.element, d, { originalEvent: t, action: i }));
			}
		} },
	    wt = { change: function change(t) {
			var i = this.options,
			    e = this.canvasData,
			    a = this.containerData,
			    o = this.cropBoxData,
			    h = this.pointers,
			    r = this.action,
			    s = i.aspectRatio,
			    c = o.left,
			    l = o.top,
			    d = o.width,
			    p = o.height,
			    m = c + d,
			    u = l + p,
			    g = 0,
			    f = 0,
			    v = a.width,
			    w = a.height,
			    x = !0,
			    b = void 0;!s && t.shiftKey && (s = d && p ? d / p : 1), this.limited && (g = o.minLeft, f = o.minTop, v = g + Math.min(a.width, e.width, e.left + e.width), w = f + Math.min(a.height, e.height, e.top + e.height));var y = h[Object.keys(h)[0]],
			    C = { x: y.endX - y.startX, y: y.endY - y.startY },
			    M = function M(t) {
				switch (t) {case "e":
						m + C.x > v && (C.x = v - m);break;case "w":
						c + C.x < g && (C.x = g - c);break;case "n":
						l + C.y < f && (C.y = f - l);break;case "s":
						u + C.y > w && (C.y = w - u);}
			};switch (r) {case "all":
					c += C.x, l += C.y;break;case "e":
					if (C.x >= 0 && (m >= v || s && (l <= f || u >= w))) {
						x = !1;break;
					}M("e"), d += C.x, s && (p = d / s, l -= C.x / s / 2), d < 0 && (r = "w", d = 0);break;case "n":
					if (C.y <= 0 && (l <= f || s && (c <= g || m >= v))) {
						x = !1;break;
					}M("n"), p -= C.y, l += C.y, s && (d = p * s, c += C.y * s / 2), p < 0 && (r = "s", p = 0);break;case "w":
					if (C.x <= 0 && (c <= g || s && (l <= f || u >= w))) {
						x = !1;break;
					}M("w"), d -= C.x, c += C.x, s && (p = d / s, l += C.x / s / 2), d < 0 && (r = "e", d = 0);break;case "s":
					if (C.y >= 0 && (u >= w || s && (c <= g || m >= v))) {
						x = !1;break;
					}M("s"), p += C.y, s && (d = p * s, c -= C.y * s / 2), p < 0 && (r = "n", p = 0);break;case "ne":
					if (s) {
						if (C.y <= 0 && (l <= f || m >= v)) {
							x = !1;break;
						}M("n"), p -= C.y, l += C.y, d = p * s;
					} else M("n"), M("e"), C.x >= 0 ? m < v ? d += C.x : C.y <= 0 && l <= f && (x = !1) : d += C.x, C.y <= 0 ? l > f && (p -= C.y, l += C.y) : (p -= C.y, l += C.y);d < 0 && p < 0 ? (r = "sw", p = 0, d = 0) : d < 0 ? (r = "nw", d = 0) : p < 0 && (r = "se", p = 0);break;case "nw":
					if (s) {
						if (C.y <= 0 && (l <= f || c <= g)) {
							x = !1;break;
						}M("n"), p -= C.y, l += C.y, d = p * s, c += C.y * s;
					} else M("n"), M("w"), C.x <= 0 ? c > g ? (d -= C.x, c += C.x) : C.y <= 0 && l <= f && (x = !1) : (d -= C.x, c += C.x), C.y <= 0 ? l > f && (p -= C.y, l += C.y) : (p -= C.y, l += C.y);d < 0 && p < 0 ? (r = "se", p = 0, d = 0) : d < 0 ? (r = "ne", d = 0) : p < 0 && (r = "sw", p = 0);break;case "sw":
					if (s) {
						if (C.x <= 0 && (c <= g || u >= w)) {
							x = !1;break;
						}M("w"), d -= C.x, c += C.x, p = d / s;
					} else M("s"), M("w"), C.x <= 0 ? c > g ? (d -= C.x, c += C.x) : C.y >= 0 && u >= w && (x = !1) : (d -= C.x, c += C.x), C.y >= 0 ? u < w && (p += C.y) : p += C.y;d < 0 && p < 0 ? (r = "ne", p = 0, d = 0) : d < 0 ? (r = "se", d = 0) : p < 0 && (r = "nw", p = 0);break;case "se":
					if (s) {
						if (C.x >= 0 && (m >= v || u >= w)) {
							x = !1;break;
						}M("e"), p = (d += C.x) / s;
					} else M("s"), M("e"), C.x >= 0 ? m < v ? d += C.x : C.y >= 0 && u >= w && (x = !1) : d += C.x, C.y >= 0 ? u < w && (p += C.y) : p += C.y;d < 0 && p < 0 ? (r = "nw", p = 0, d = 0) : d < 0 ? (r = "sw", d = 0) : p < 0 && (r = "ne", p = 0);break;case "move":
					this.move(C.x, C.y), x = !1;break;case "zoom":
					this.zoom(function (t) {
						var i = S({}, t),
						    e = [];return O(t, function (t, a) {
							delete i[a], O(i, function (i) {
								var a = Math.abs(t.startX - i.startX),
								    n = Math.abs(t.startY - i.startY),
								    o = Math.abs(t.endX - i.endX),
								    h = Math.abs(t.endY - i.endY),
								    r = Math.sqrt(a * a + n * n),
								    s = (Math.sqrt(o * o + h * h) - r) / r;e.push(s);
							});
						}), e.sort(function (t, i) {
							return Math.abs(t) < Math.abs(i);
						}), e[0];
					}(h), t), x = !1;break;case "crop":
					if (!C.x || !C.y) {
						x = !1;break;
					}b = tt(this.cropper), c = y.startX - b.left, l = y.startY - b.top, d = o.minWidth, p = o.minHeight, C.x > 0 ? r = C.y > 0 ? "se" : "ne" : C.x < 0 && (c -= d, r = C.y > 0 ? "sw" : "nw"), C.y < 0 && (l -= p), this.cropped || (P(this.cropBox, n), this.cropped = !0, this.limited && this.limitCropBox(!0, !0));}x && (o.width = d, o.height = p, o.left = c, o.top = l, this.action = r, this.renderCropBox()), O(h, function (t) {
				t.startX = t.endX, t.startY = t.endY;
			});
		} },
	    xt = { crop: function crop() {
			return this.ready && !this.disabled && (this.cropped || (this.cropped = !0, this.limitCropBox(!0, !0), this.options.modal && j(this.dragBox, h), P(this.cropBox, n)), this.setCropBoxData(this.initialCropBoxData)), this;
		}, reset: function reset() {
			return this.ready && !this.disabled && (this.imageData = S({}, this.initialImageData), this.canvasData = S({}, this.initialCanvasData), this.cropBoxData = S({}, this.initialCropBoxData), this.renderCanvas(), this.cropped && this.renderCropBox()), this;
		}, clear: function clear() {
			return this.cropped && !this.disabled && (S(this.cropBoxData, { left: 0, top: 0, width: 0, height: 0 }), this.cropped = !1, this.renderCropBox(), this.limitCanvas(!0, !0), this.renderCanvas(), P(this.dragBox, h), j(this.cropBox, n)), this;
		}, replace: function replace(t) {
			var i = arguments.length > 1 && void 0 !== arguments[1] && arguments[1];return !this.disabled && t && (this.isImg && (this.element.src = t), i ? (this.url = t, this.image.src = t, this.ready && (this.image2.src = t, O(this.previews, function (i) {
				i.getElementsByTagName("img")[0].src = t;
			}))) : (this.isImg && (this.replaced = !0), this.options.data = null, this.load(t))), this;
		}, enable: function enable() {
			return this.ready && (this.disabled = !1, P(this.cropper, a)), this;
		}, disable: function disable() {
			return this.ready && (this.disabled = !0, j(this.cropper, a)), this;
		}, destroy: function destroy() {
			var t = this.element,
			    e = this.image;return this.loaded ? (this.isImg && this.replaced && (t.src = this.originalUrl), this.unbuild(), P(t, n)) : this.isImg ? G(t, u, this.onStart) : e && e.parentNode.removeChild(e), K(t, i), this;
		}, move: function move(t, i) {
			var e = this.canvasData,
			    a = e.left,
			    n = e.top;return this.moveTo(N(t) ? t : a + Number(t), N(i) ? i : n + Number(i));
		}, moveTo: function moveTo(t) {
			var i = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : t,
			    e = this.canvasData,
			    a = !1;return t = Number(t), i = Number(i), this.ready && !this.disabled && this.options.movable && (W(t) && (e.left = t, a = !0), W(i) && (e.top = i, a = !0), a && this.renderCanvas(!0)), this;
		}, zoom: function zoom(t, i) {
			var e = this.canvasData;return t = (t = Number(t)) < 0 ? 1 / (1 - t) : 1 + t, this.zoomTo(e.width * t / e.naturalWidth, null, i);
		}, zoomTo: function zoomTo(t, i, e) {
			var a = this.options,
			    n = this.canvasData,
			    o = n.width,
			    h = n.height,
			    r = n.naturalWidth,
			    s = n.naturalHeight;if ((t = Number(t)) >= 0 && this.ready && !this.disabled && a.zoomable) {
				var c = r * t,
				    l = s * t;if (!1 === _(this.element, "zoom", { originalEvent: e, oldRatio: o / r, ratio: c / r })) return this;if (e) {
					var d = this.pointers,
					    p = tt(this.cropper),
					    m = d && Object.keys(d).length ? function (t) {
						var i = 0,
						    e = 0,
						    a = 0;return O(t, function (t) {
							var n = t.startX,
							    o = t.startY;i += n, e += o, a += 1;
						}), { pageX: i /= a, pageY: e /= a };
					}(d) : { pageX: e.pageX, pageY: e.pageY };n.left -= (c - o) * ((m.pageX - p.left - n.left) / o), n.top -= (l - h) * ((m.pageY - p.top - n.top) / h);
				} else Y(i) && W(i.x) && W(i.y) ? (n.left -= (c - o) * ((i.x - n.left) / o), n.top -= (l - h) * ((i.y - n.top) / h)) : (n.left -= (c - o) / 2, n.top -= (l - h) / 2);n.width = c, n.height = l, this.renderCanvas(!0);
			}return this;
		}, rotate: function rotate(t) {
			return this.rotateTo((this.imageData.rotate || 0) + Number(t));
		}, rotateTo: function rotateTo(t) {
			return W(t = Number(t)) && this.ready && !this.disabled && this.options.rotatable && (this.imageData.rotate = t % 360, this.renderCanvas(!0, !0)), this;
		}, scaleX: function scaleX(t) {
			var i = this.imageData.scaleY;return this.scale(t, W(i) ? i : 1);
		}, scaleY: function scaleY(t) {
			var i = this.imageData.scaleX;return this.scale(W(i) ? i : 1, t);
		}, scale: function scale(t) {
			var i = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : t,
			    e = this.imageData,
			    a = !1;return t = Number(t), i = Number(i), this.ready && !this.disabled && this.options.scalable && (W(t) && (e.scaleX = t, a = !0), W(i) && (e.scaleY = i, a = !0), a && this.renderCanvas(!0, !0)), this;
		}, getData: function getData() {
			var t = arguments.length > 0 && void 0 !== arguments[0] && arguments[0],
			    i = this.options,
			    e = this.imageData,
			    a = this.canvasData,
			    n = this.cropBoxData,
			    o = void 0;if (this.ready && this.cropped) {
				o = { x: n.left - a.left, y: n.top - a.top, width: n.width, height: n.height };var h = e.width / e.naturalWidth;O(o, function (i, e) {
					i /= h, o[e] = t ? Math.round(i) : i;
				});
			} else o = { x: 0, y: 0, width: 0, height: 0 };return i.rotatable && (o.rotate = e.rotate || 0), i.scalable && (o.scaleX = e.scaleX || 1, o.scaleY = e.scaleY || 1), o;
		}, setData: function setData(t) {
			var i = this.options,
			    e = this.imageData,
			    a = this.canvasData,
			    n = {};if (X(t) && (t = t.call(this.element)), this.ready && !this.disabled && Y(t)) {
				var o = !1;i.rotatable && W(t.rotate) && t.rotate !== e.rotate && (e.rotate = t.rotate, o = !0), i.scalable && (W(t.scaleX) && t.scaleX !== e.scaleX && (e.scaleX = t.scaleX, o = !0), W(t.scaleY) && t.scaleY !== e.scaleY && (e.scaleY = t.scaleY, o = !0)), o && this.renderCanvas(!0, !0);var h = e.width / e.naturalWidth;W(t.x) && (n.left = t.x * h + a.left), W(t.y) && (n.top = t.y * h + a.top), W(t.width) && (n.width = t.width * h), W(t.height) && (n.height = t.height * h), this.setCropBoxData(n);
			}return this;
		}, getContainerData: function getContainerData() {
			return this.ready ? S({}, this.containerData) : {};
		}, getImageData: function getImageData() {
			return this.loaded ? S({}, this.imageData) : {};
		}, getCanvasData: function getCanvasData() {
			var t = this.canvasData,
			    i = {};return this.ready && O(["left", "top", "width", "height", "naturalWidth", "naturalHeight"], function (e) {
				i[e] = t[e];
			}), i;
		}, setCanvasData: function setCanvasData(t) {
			var i = this.canvasData,
			    e = i.aspectRatio;return X(t) && (t = t.call(this.element)), this.ready && !this.disabled && Y(t) && (W(t.left) && (i.left = t.left), W(t.top) && (i.top = t.top), W(t.width) ? (i.width = t.width, i.height = t.width / e) : W(t.height) && (i.height = t.height, i.width = t.height * e), this.renderCanvas(!0)), this;
		}, getCropBoxData: function getCropBoxData() {
			var t = this.cropBoxData,
			    i = void 0;return this.ready && this.cropped && (i = { left: t.left, top: t.top, width: t.width, height: t.height }), i || {};
		}, setCropBoxData: function setCropBoxData(t) {
			var i = this.cropBoxData,
			    e = this.options.aspectRatio,
			    a = void 0,
			    n = void 0;return X(t) && (t = t.call(this.element)), this.ready && this.cropped && !this.disabled && Y(t) && (W(t.left) && (i.left = t.left), W(t.top) && (i.top = t.top), W(t.width) && t.width !== i.width && (a = !0, i.width = t.width), W(t.height) && t.height !== i.height && (n = !0, i.height = t.height), e && (a ? i.height = i.width / e : n && (i.width = i.height * e)), this.renderCropBox()), this;
		}, getCroppedCanvas: function getCroppedCanvas() {
			var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {};if (!this.ready || !window.HTMLCanvasElement) return null;var i = this.canvasData,
			    e = function (t, i, e, a) {
				var n = i.naturalWidth,
				    o = i.naturalHeight,
				    h = i.rotate,
				    r = void 0 === h ? 0 : h,
				    s = i.scaleX,
				    c = void 0 === s ? 1 : s,
				    l = i.scaleY,
				    d = void 0 === l ? 1 : l,
				    p = e.aspectRatio,
				    m = e.naturalWidth,
				    u = e.naturalHeight,
				    g = a.fillColor,
				    f = void 0 === g ? "transparent" : g,
				    v = a.imageSmoothingEnabled,
				    w = void 0 === v || v,
				    x = a.imageSmoothingQuality,
				    b = void 0 === x ? "low" : x,
				    y = a.maxWidth,
				    C = void 0 === y ? 1 / 0 : y,
				    M = a.maxHeight,
				    D = void 0 === M ? 1 / 0 : M,
				    B = a.minWidth,
				    k = void 0 === B ? 0 : B,
				    T = a.minHeight,
				    W = void 0 === T ? 0 : T,
				    N = document.createElement("canvas"),
				    H = N.getContext("2d"),
				    L = lt({ aspectRatio: p, width: C, height: D }),
				    Y = lt({ aspectRatio: p, width: k, height: W }),
				    X = Math.min(L.width, Math.max(Y.width, m)),
				    O = Math.min(L.height, Math.max(Y.height, u)),
				    S = [-n / 2, -o / 2, n, o];return N.width = A(X), N.height = A(O), H.fillStyle = f, H.fillRect(0, 0, X, O), H.save(), H.translate(X / 2, O / 2), H.rotate(r * Math.PI / 180), H.scale(c, d), H.imageSmoothingEnabled = w, H.imageSmoothingQuality = b, H.drawImage.apply(H, [t].concat(E(S.map(function (t) {
					return Math.floor(A(t));
				})))), H.restore(), N;
			}(this.image, this.imageData, i, t);if (!this.cropped) return e;var a = this.getData(),
			    n = a.x,
			    o = a.y,
			    h = a.width,
			    r = a.height,
			    s = h / r,
			    c = lt({ aspectRatio: s, width: t.maxWidth || 1 / 0, height: t.maxHeight || 1 / 0 }),
			    l = lt({ aspectRatio: s, width: t.minWidth || 0, height: t.minHeight || 0 }),
			    d = lt({ aspectRatio: s, width: t.width || h, height: t.height || r }),
			    p = d.width,
			    m = d.height;p = Math.min(c.width, Math.max(l.width, p)), m = Math.min(c.height, Math.max(l.height, m));var u = document.createElement("canvas"),
			    g = u.getContext("2d");u.width = A(p), u.height = A(m), g.fillStyle = t.fillColor || "transparent", g.fillRect(0, 0, p, m);var f = t.imageSmoothingEnabled,
			    v = void 0 === f || f,
			    w = t.imageSmoothingQuality;g.imageSmoothingEnabled = v, w && (g.imageSmoothingQuality = w);var x = e.width,
			    b = e.height,
			    y = n,
			    C = o,
			    M = void 0,
			    D = void 0,
			    B = void 0,
			    k = void 0,
			    T = void 0,
			    W = void 0;y <= -h || y > x ? (y = 0, M = 0, B = 0, T = 0) : y <= 0 ? (B = -y, y = 0, T = M = Math.min(x, h + y)) : y <= x && (B = 0, T = M = Math.min(h, x - y)), M <= 0 || C <= -r || C > b ? (C = 0, D = 0, k = 0, W = 0) : C <= 0 ? (k = -C, C = 0, W = D = Math.min(b, r + C)) : C <= b && (k = 0, W = D = Math.min(r, b - C));var N = [y, C, M, D];if (T > 0 && W > 0) {
				var H = p / h;N.push(B * H, k * H, T * H, W * H);
			}return g.drawImage.apply(g, [e].concat(E(N.map(function (t) {
				return Math.floor(A(t));
			})))), u;
		}, setAspectRatio: function setAspectRatio(t) {
			var i = this.options;return this.disabled || N(t) || (i.aspectRatio = Math.max(0, t) || NaN, this.ready && (this.initCropBox(), this.cropped && this.renderCropBox())), this;
		}, setDragMode: function setDragMode(t) {
			var i = this.options,
			    a = this.dragBox,
			    n = this.face;if (this.loaded && !this.disabled) {
				var o = "crop" === t,
				    h = i.movable && "move" === t;F(a, s, t = o || h ? t : "none"), q(a, e, o), q(a, r, h), i.cropBoxMovable || (F(n, s, t), q(n, e, o), q(n, r, h));
			}return this;
		} },
	    bt = t.Cropper,
	    yt = function () {
		function t(i) {
			var e = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};if (B(this, t), !i || !C.test(i.tagName)) throw new Error("The first argument is required and must be an <img> or <canvas> element.");this.element = i, this.options = S({}, M, Y(e) && e), this.complete = !1, this.cropped = !1, this.disabled = !1, this.isImg = !1, this.limited = !1, this.loaded = !1, this.ready = !1, this.replaced = !1, this.wheeling = !1, this.originalUrl = "", this.canvasData = null, this.cropBoxData = null, this.previews = null, this.pointers = {}, this.init();
		}return k(t, [{ key: "init", value: function value() {
				var t = this.element,
				    e = t.tagName.toLowerCase(),
				    a = void 0;if (!Z(t, i)) {
					if (F(t, i, this), "img" === e) {
						if (this.isImg = !0, a = t.getAttribute("src") || "", this.originalUrl = a, !a) return;a = t.src;
					} else "canvas" === e && window.HTMLCanvasElement && (a = t.toDataURL());this.load(a);
				}
			} }, { key: "load", value: function value(t) {
				var i = this;if (t) {
					this.url = t, this.imageData = {};var e = this.element,
					    a = this.options;if (a.checkOrientation && window.ArrayBuffer) {
						if (b.test(t)) y.test(t) ? this.read(function (t) {
							var i = t.replace(pt, ""),
							    e = atob(i),
							    a = new ArrayBuffer(e.length),
							    n = new Uint8Array(a);return O(n, function (t, i) {
								n[i] = e.charCodeAt(i);
							}), a;
						}(t)) : this.clone();else {
							var n = new XMLHttpRequest();n.onerror = function () {
								i.clone();
							}, n.onload = function () {
								i.read(n.response);
							}, a.checkCrossOrigin && at(t) && e.crossOrigin && (t = nt(t)), n.open("get", t), n.responseType = "arraybuffer", n.withCredentials = "use-credentials" === e.crossOrigin, n.send();
						}
					} else this.clone();
				}
			} }, { key: "read", value: function value(t) {
				var i = this.options,
				    e = this.imageData,
				    a = mt(t),
				    n = 0,
				    o = 1,
				    h = 1;if (a > 1) {
					this.url = function (t, i) {
						var e = "";return O(new Uint8Array(t), function (t) {
							e += dt(t);
						}), "data:" + i + ";base64," + btoa(e);
					}(t, "image/jpeg");var r = function (t) {
						var i = 0,
						    e = 1,
						    a = 1;switch (t) {case 2:
								e = -1;break;case 3:
								i = -180;break;case 4:
								a = -1;break;case 5:
								i = 90, a = -1;break;case 6:
								i = 90;break;case 7:
								i = 90, e = -1;break;case 8:
								i = -90;}return { rotate: i, scaleX: e, scaleY: a };
					}(a);n = r.rotate, o = r.scaleX, h = r.scaleY;
				}i.rotatable && (e.rotate = n), i.scalable && (e.scaleX = o, e.scaleY = h), this.clone();
			} }, { key: "clone", value: function value() {
				var t = this.element,
				    i = this.url,
				    e = void 0,
				    a = void 0;this.options.checkCrossOrigin && at(i) && ((e = t.crossOrigin) ? a = i : (e = "anonymous", a = nt(i))), this.crossOrigin = e, this.crossOriginUrl = a;var n = document.createElement("img");e && (n.crossOrigin = e), n.src = a || i;var h = z(this.start, this),
				    r = z(this.stop, this);this.image = n, this.onStart = h, this.onStop = r, this.isImg ? t.complete ? this.start() : J(t, u, h) : (J(n, u, h), J(n, "error", r), j(n, o), t.parentNode.insertBefore(n, t.nextSibling));
			} }, { key: "start", value: function value(t) {
				var i = this,
				    e = this.isImg ? this.element : this.image;t && (G(e, u, this.onStart), G(e, "error", this.onStop)), function (t, i) {
					if (!t.naturalWidth || rt) {
						var e = document.createElement("img"),
						    a = document.body || document.documentElement;e.onload = function () {
							i(e.width, e.height), rt || a.removeChild(e);
						}, e.src = t.src, rt || (e.style.cssText = "left:0;max-height:none!important;max-width:none!important;min-height:0!important;min-width:0!important;opacity:0;position:absolute;top:0;z-index:-1;", a.appendChild(e));
					} else i(t.naturalWidth, t.naturalHeight);
				}(e, function (t, e) {
					S(i.imageData, { naturalWidth: t, naturalHeight: e, aspectRatio: t / e }), i.loaded = !0, i.build();
				});
			} }, { key: "stop", value: function value() {
				var t = this.image;G(t, u, this.onStart), G(t, "error", this.onStop), t.parentNode.removeChild(t), this.image = null;
			} }, { key: "build", value: function value() {
				var t = this;if (this.loaded) {
					this.ready && this.unbuild();var e = this.element,
					    a = this.options,
					    c = this.image,
					    d = e.parentNode,
					    p = document.createElement("div");p.innerHTML = '<div class="cropper-container"><div class="cropper-wrap-box"><div class="cropper-canvas"></div></div><div class="cropper-drag-box"></div><div class="cropper-crop-box"><span class="cropper-view-box"></span><span class="cropper-dashed dashed-h"></span><span class="cropper-dashed dashed-v"></span><span class="cropper-center"></span><span class="cropper-face"></span><span class="cropper-line line-e" data-action="e"></span><span class="cropper-line line-n" data-action="n"></span><span class="cropper-line line-w" data-action="w"></span><span class="cropper-line line-s" data-action="s"></span><span class="cropper-point point-e" data-action="e"></span><span class="cropper-point point-n" data-action="n"></span><span class="cropper-point point-w" data-action="w"></span><span class="cropper-point point-s" data-action="s"></span><span class="cropper-point point-ne" data-action="ne"></span><span class="cropper-point point-nw" data-action="nw"></span><span class="cropper-point point-sw" data-action="sw"></span><span class="cropper-point point-se" data-action="se"></span></div></div>';var m = p.querySelector("." + i + "-container"),
					    u = m.querySelector("." + i + "-canvas"),
					    g = m.querySelector("." + i + "-drag-box"),
					    f = m.querySelector("." + i + "-crop-box"),
					    v = f.querySelector("." + i + "-face");this.container = d, this.cropper = m, this.canvas = u, this.dragBox = g, this.cropBox = f, this.viewBox = m.querySelector("." + i + "-view-box"), this.face = v, u.appendChild(c), j(e, n), d.insertBefore(m, e.nextSibling), this.isImg || P(c, o), this.initPreview(), this.bind(), a.aspectRatio = Math.max(0, a.aspectRatio) || NaN, a.viewMode = Math.max(0, Math.min(3, Math.round(a.viewMode))) || 0, this.cropped = a.autoCrop, a.autoCrop ? a.modal && j(g, h) : j(f, n), a.guides || j(f.getElementsByClassName(i + "-dashed"), n), a.center || j(f.getElementsByClassName(i + "-center"), n), a.background && j(m, i + "-bg"), a.highlight || j(v, "cropper-invisible"), a.cropBoxMovable && (j(v, r), F(v, s, "all")), a.cropBoxResizable || (j(f.getElementsByClassName(i + "-line"), n), j(f.getElementsByClassName(i + "-point"), n)), this.setDragMode(a.dragMode), this.render(), this.ready = !0, this.setData(a.data), this.completing = setTimeout(function () {
						X(a.ready) && J(e, "ready", a.ready, { once: !0 }), _(e, "ready"), _(e, l, t.getData()), t.complete = !0;
					}, 0);
				}
			} }, { key: "unbuild", value: function value() {
				this.ready && (this.complete || clearTimeout(this.completing), this.ready = !1, this.complete = !1, this.initialImageData = null, this.initialCanvasData = null, this.initialCropBoxData = null, this.containerData = null, this.canvasData = null, this.cropBoxData = null, this.unbind(), this.resetPreview(), this.previews = null, this.viewBox = null, this.cropBox = null, this.dragBox = null, this.canvas = null, this.container = null, this.cropper.parentNode.removeChild(this.cropper), this.cropper = null);
			} }], [{ key: "noConflict", value: function value() {
				return window.Cropper = bt, t;
			} }, { key: "setDefaults", value: function value(t) {
				S(M, Y(t) && t);
			} }]), t;
	}();return S(yt.prototype, ut, gt, ft, vt, wt, xt), yt;
});

var huiImgCuter = function () {
	var _huiImgCuter = function _huiImgCuter(aspectRatio, saveWidth) {
		if (!aspectRatio) {
			aspectRatio = 1;
		}
		if (!saveWidth) {
			saveWidth = 200;
		}
		//添加选择域
		var inputObj = hui.createDom('input');
		inputObj.setAttribute('type', 'file');
		inputObj.setAttribute('accept', 'image/*');
		inputObj.setAttribute('id', 'hui-img-cuter-file');
		hui(inputObj).appendTo('#hui-img-cuter-select');
		//绑定选择图片按钮
		this.bindSelect = function (selector) {
			hui(selector).click(function () {
				var a = document.createEvent("MouseEvents");
				a.initEvent("click", true, true);
				document.getElementById("hui-img-cuter-file").dispatchEvent(a);
			});
		};
		//初始化
		this.Cuter = hui.createDom('div', { id: 'hui-img-cuter' });
		this.Cuter.innerHTML = '<div id="hui-img-cuter-img"></div>';
		document.body.appendChild(this.Cuter);
		this.Cuter = hui('#hui-img-cuter');
		this.CuterImg = hui('#hui-img-cuter-img');
		hui('#hui-img-cuter-file').change(function () {
			hui.loading('加载图片...');
			var reader = new FileReader();
			reader.onload = function (e) {
				_cuterSelf.CuterImg.html('<img src="' + e.target.result + '" />');
				_cuterSelf.cropper = new Cropper(_cuterSelf.CuterImg.find('img').eq(0).dom[0], {
					resizable: false,
					aspectRatio: aspectRatio,
					ready: function ready() {
						croppable = true;
					}
				});
				hui.closeLoading();
			};
			if (this.files[0]) {
				reader.readAsDataURL(this.files[0]);
			} else {
				hui.closeLoading();
			}
		});
		this.getImgData = function () {
			if (!_cuterSelf.cropper) {
				return false;
			}
			var canvas = _cuterSelf.cropper.getCroppedCanvas({ width: saveWidth, height: saveWidth * aspectRatio });
			return canvas.toDataURL();
		};
		_cuterSelf = this;
	};
	return _huiImgCuter;
}();