listModule.controller("MenuController", function ($scope, infoservice) {
    $scope.sections = infoservice.showSection;
    //$scope.users = userservice.users;

    var onError = function (data) {
        console.log(data);
    };

    //sections
    $scope.addSection = function (section) {
        $scope.sections.push(section);
    }
    
    if ($scope.sections.length == 0) {
        $scope.addSection({ type: "user", shown: true });
        $scope.addSection({ type: "view", shown: true });
        $scope.addSection({ type: "viewitems", shown: true });
        $scope.addSection({ type: "list", shown: true });
        $scope.addSection({ type: "listitems", shown: true });
    }
    $scope.isSectionVisible = function (sectiontype) {
        var idx;
        for (idx in $scope.sections) {
            var section = $scope.sections[idx];
            if (section.type == sectiontype) {
                return section.shown;
            }
        }
    }
    $scope.setSectionVisible = function (sectiontype) {
        var idx;
        for (idx in $scope.sections) {
            var section = $scope.sections[idx];
            if (section.type == sectiontype) {
                section.shown = !section.shown;
            }
        }
    };

});
