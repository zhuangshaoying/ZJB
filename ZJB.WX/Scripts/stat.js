/*线型图标*/
function init_trend_chart(time_period_ary, success_ary, fail_ary) {

	$('#lookAsCount-time').highcharts({
	    chart: {
	        type: 'line'
	    },
	    title: {
	        text: '最近15天推送统计'
	    },
	    xAxis: {
	        categories: time_period_ary
	    },
	    yAxis: {
	        title: {
	            text: '次数'
	        }
	    },
	    tooltip: {
	        enabled: false,
	        formatter: function() {
	            return '<b>'+ this.series.name +'</b><br/>'+
	                this.x +': '+ this.y +'°C';
	        }
	    },
	    plotOptions: {
	        line: {
	            dataLabels: {
	                enabled: true,
	                style: {
	                    textShadow: '0 0 3px white, 0 0 3px white'
	                }
	            },
	            enableMouseTracking: false
	        }
	    },
	    series: [{
	        name: '成功',
	        data: success_ary
	    }, {
	        name: '失败',
	        data: fail_ary
	    }]
	});
}


/*饼状图表*/
var chart;

function init_pie_chart(web_ary) {	
    $('#lookAsCount-website').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: '目标网站推送成功数统计'
        },
        tooltip: {
    	    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: '网站发送统计占比',
            data: web_ary
        }]
    });
}