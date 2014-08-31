var infoservice = function ($http) {
    //menu
    var showSection = [];
    //select
    var saveSelect = function (select) {
        return $http.post("/API/InfoSelect/", { isselect: select.isselect, id: select.id })
        .then(function (response) {
            return response.data;
        });
    }

    //remove extra chars from attrib
    var cleanAttrib = function (attrib) {
        try{
        var attrib1 = replaceAll(attrib, "\"{", "{");
        var attrib2 = replaceAll(attrib1, "\\\"", "\"");
        var attrib3 = replaceAll(attrib2, "}\"", "}");
        return JSON.parse(attrib3);
        }
        catch(ex)
        {
            return {};
        }
    }
    //attribs
    var replaceAll = function (orig, find, replace) {
        while (orig.indexOf(find) >= 0) {
            var newstr = orig.replace(find, replace);
            orig = newstr;
        }
        return orig;
    };

    var saveAttrib = function (attribObj) {
        var attribs = [];
        attribs.push({
            id: attribObj.attrib.id,
            attrib: JSON.stringify(attribObj.attrib.attrib)
        });        
        return $http.post("/API/InfoAttrib/", attribs)
        .then(function (response) {
            return response.data;
        });
    }
    var saveAttribs = function (attribs) {
        return $http.post("/API/InfoAttrib/", attribs)
        .then(function (response) {
            return response.data;
        });
    }

    return {
        saveSelect: saveSelect,
        showSection: showSection,
        cleanAttrib: cleanAttrib,
        saveAttribs: saveAttribs,
        saveAttrib: saveAttrib
    }

}

listModule.factory("infoservice", infoservice);
