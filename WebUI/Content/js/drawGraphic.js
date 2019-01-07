
function showTooltip(x, y, contents, color) 
{
    $('<div id="tooltip">' + contents + '</div>').css( {
        position: 'absolute',
        display: 'none',
        top: y + 5,
        left: x + 5,
        border: '2px solid',
        padding: '2px',
        'background-color': '#fff',
        opacity: 0.80,
        '-moz-border-radius': '5px',
        '-webkit-border-radius': '5px',
        'border-radius': '5px',
        'border-color': color,
        }).appendTo("body").fadeIn(200);
}

function drawGraphic( getdataurl, iswarningurl, groupid, xaxislabel, yaxislabel )
{
        var placeholder = document.getElementById('placeholder-' + groupid)
        var options = {
            legend: {
                noColumns: 3,
                container: $('#legend-' + groupid)
            },
            grid: {
                borderWidth: 1,
                //minBorderMargin: 20,
                //labelMargin: 10,
                hoverable: true,
                margin: {
                    top: 8,
                    bottom: 20,
                    left: 20
                    }
            },
            yaxis: { min: 0 },
            xaxis: { mode: 'time',
                     timeformat: '%H:%M\n%d.%m',
                     tickSize: [1, 'hour'],
                     minTickSize: [1, 'hour'] }
            //selection: { mode: 'x' }
        };
        var PLOT = $.plot(placeholder, [], options);

        var previousPoint = null;
        $('#placeholder-'+groupid).bind('plothover', function (event, pos, item) {
            if (item) {
                if (previousPoint != item.dataIndex) {
                    previousPoint = item.dataIndex;
                    
                    $('#tooltip').remove();
                    var x = item.datapoint[0].toFixed(2);
                    var y = item.datapoint[1].toFixed(2);
                    var color = item.series.color;
                    showTooltip(item.pageX, item.pageY,
                                item.series.label + " = " + y,
                                color);
                }
            }
            else {
                    $('#tooltip').remove();
                    previousPoint = null;            
            }
        });
        /*$('#placeholder-' + groupid).bind('plotselected', function (event, ranges) {
            PLOT = $.plot(placeholder, PLOT.getData(),
                          $.extend(true, {}, options, {
                              xaxis: { min: ranges.xaxis.from, max: ranges.xaxis.to }
                          }));
                          // add zoom out button 
                          $('<div class="btn btn-small" style="right:20px;top:20px;position:absolute;"><i class="icon-zoom-out"></i></div>').appendTo(placeholder).click(function (e) {
                              e.preventDefault();
                              PLOT = $.plot(placeholder, PLOT.getData(), options);
                          });
        });*/

         //Create the demo X and Y axis labels
         var xaxisLabel = $("<div class='axisLabel xaxisLabel'></div>")
         .text(xaxislabel)
         .appendTo(placeholder);
        var yaxisLabel = $("<div class='axisLabel yaxisLabel'></div>")
        .text(yaxislabel)
        .appendTo(placeholder);
        // Since CSS transforms use the top-left corner of the label as the transform origin,
        // we need to center the y-axis label by shifting it down by half its width.
        // Subtract 20 to factor the chart's bottom margin into the centering.
        yaxisLabel.css("margin-top", yaxisLabel.width() / 2 - 20);
        // Update the random dataset at 25FPS for a smoothly-animating chart 

        function update() {
            $.getJSON(getdataurl, { 'groupID': groupid }, function (json) {

                $.getJSON(iswarningurl, { 'groupID': groupid }, function (json) {
                    var indicator = document.getElementById('indicator-' + groupid)
                    if (json) {
                        indicator.style.visibility = 'visible';
                        $('#indicator-' + groupid).effect('pulsate', { times: 5 }, 300);
                    }
                    else {
                        indicator.style.visibility = 'hidden';
                    }
                });

                var dataset = [];
                var i = 1;
                for (key in json) {
                    //if (key.toLowerCase().indexOf('факт') >= 0) { 
                        var bar = { label: key, data: json[key], bars: { show: true, barWidth : 20*60*1000 , fill: true, order: i } }
                        dataset.push(bar);
                        i++;
//                    }
//                    else {
//                        var line = { label: key, data: json[key], lines: { show: true, lineWidth: 2}, points: { show: true} }
//                        dataset.push(line);
//                    } 
                }
                PLOT.setData(dataset);
                PLOT.setupGrid();
                PLOT.draw();
            });

            setTimeout(update, 15000);
        }
        update(); 
}