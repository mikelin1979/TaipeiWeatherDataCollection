angular.module("PWEBApp", ["chart.js"]).controller("LineCtrl", function ($scope, $http) {
            
    $scope.Area = "";   //已選地區
    $scope.STTime = ""; //已選起始時間
    $scope.EDTime = ""; //已選結束時間
    $scope.ListArea = []; //地區下拉
    $scope.ListStartTime = []; //起始時間下拉
    $scope.ListEndTime = []; //結束時間下拉

    $scope.datasetOverride = [{ yAxisID: 'y-axis-1' }];
    $scope.options = {
        scales: {
            yAxes: [
                {
                    id: 'y-axis-1',
                    type: 'linear',
                    display: true,
                    position: 'left'
                }
            ]
        }
    };

       

    //取得數據
    $scope.getdata = function (postdata) {
        $http.post('/Home/Query', postdata).then(function successCallback(response) {
            $scope.VM = response.data;

            $scope.labels = $scope.VM.label;
            $scope.series = $scope.VM.series;
            $scope.data = $scope.VM.data;
            $scope.ListArea = $scope.VM.area;
            $scope.ListStartTime = $scope.VM.listStartTime;
            $scope.ListEndTime = $scope.VM.listEndTime;
        }, function errorCallback(response) {
            console.log(response);
        });        
    };

    $scope.onchange = function () {
        $scope.getdata({ location: $scope.Area, sttime: $scope.STTime, edtime: $scope.EDTime });
    };

    //取得初始數據
    $scope.getDefVM = function () {
        $scope.getdata({ location: $scope.Area, sttime: $scope.STTime, edtime: $scope.EDTime });
    }; 

    //取得初始數據
    $scope.getDefVM();
});
