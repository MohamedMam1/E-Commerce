

(function ($) {
    "use strict";
    function extractGrid(html) {
        var $doc = $($.parseHTML(html, document, true));
        var $grid = $doc.filter(".isotope-grid").add($doc.find(".isotope-grid")).first();
        return $grid.length ? $grid.html() : null;
    }

    function reinitIsotope() {
        var $grid = $(".isotope-grid");

        if ($grid.data("isotope")) {
            $grid.isotope("destroy");
        }
        $grid.isotope({
            itemSelector: ".isotope-item",
            layoutMode: "fitRows",
            percentPosition: true,
            animationEngine: "best-available",
            masonry: { columnWidth: ".isotope-item" }
        });
      
        $(".filter-tope-group").off("click.livefilter").on("click.livefilter", "button", function () {
            var filterValue = $(this).attr("data-filter");
            $(".isotope-grid").isotope({ filter: filterValue });

            $(".filter-tope-group button").removeClass("how-active1");
            $(this).addClass("how-active1");
        });
    }

    function showLoading() {
        if (!$("#live-loading-overlay").length) {
            $(".isotope-grid").css("position", "relative").prepend(
                '<div id="live-loading-overlay" style="' +
                    "position:absolute;inset:0;background:rgba(255,255,255,0.55);" +
                    "display:flex;align-items:center;justify-content:center;" +
                    'z-index:10;border-radius:4px">' +
                    '<div style="width:28px;height:28px;border:3px solid #ccc;' +
                    "border-top-color:#555;border-radius:50%;" +
                    'animation:liveSpinner 0.7s linear infinite"></div></div>'
            );
            if (!document.getElementById("live-spinner-style")) {
                var style = document.createElement("style");
                style.id = "live-spinner-style";
                style.textContent =
                    "@keyframes liveSpinner{to{transform:rotate(360deg)}}";
                document.head.appendChild(style);
            }
        }
        $("#live-loading-overlay").show();
    }

    function hideLoading() {
        $("#live-loading-overlay").hide();
    }

    function showEmptyState(message) {
        $(".isotope-grid").html(
            '<div class="col-12 text-center p-t-50 p-b-50" style="color:#888;font-size:16px">' +
                (message || "No products found.") +
                "</div>"
        );
    }

    function fetchAndSwap(url, activeLink) {
        showLoading();

        $.ajax({
            url: url,
            type: "GET",
            success: function (html) {
                var gridHtml = extractGrid(html);
                if (gridHtml === null) {
                    showEmptyState();
                    hideLoading();
                    return;
                }

                $(".isotope-grid").html(gridHtml);
                hideLoading();
                reinitIsotope();

                if (activeLink) {
                    $(".panel-filter .filter-link").removeClass("filter-link-active");
                    $(activeLink).addClass("filter-link-active");
                }
            },
            error: function () {
                hideLoading();
                showEmptyState("Something went wrong. Please try again.");
            }
        });
    }

    var searchTimer = null;
    var lastSearchValue = "";

    $(document).on("input", 'input[name="SearchValue"]', function () {
        var val = $(this).val().trim();

        if (val === lastSearchValue) return;
        lastSearchValue = val;

        clearTimeout(searchTimer);

        if (val === "") {
            searchTimer = setTimeout(function () {
                fetchAndSwap("/Product/Index");
            }, 200);
            return;
        }

        searchTimer = setTimeout(function () {
            fetchAndSwap("/Product/Search?SearchValue=" + encodeURIComponent(val));
        }, 350);
    });

    $(document).on("submit", "form[action*='Search']", function (e) {
        e.preventDefault();
        var val = $(this).find('input[name="SearchValue"]').val().trim();
        if (val) {
            fetchAndSwap("/Product/Search?SearchValue=" + encodeURIComponent(val));
        } else {
            fetchAndSwap("/Product/Index");
        }
    });

    $(document).on("click.livefilter", ".panel-filter a.filter-link", function (e) {
        e.preventDefault();
        var url = $(this).attr("href");
        if (!url || url === "#") return;
        fetchAndSwap(url, this);
    });

    $(document).on("click.livefilter", ".panel-filter a[href*='/Product/Index']", function (e) {
        e.preventDefault();
        $(".panel-filter .filter-link").removeClass("filter-link-active");
        $(this).addClass("filter-link-active");
        fetchAndSwap("/Product/Index");
    });

    $(window).on("load", function () {
        reinitIsotope();
    });

})(jQuery);
