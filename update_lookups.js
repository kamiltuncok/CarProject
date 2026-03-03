const fs = require('fs');
const path = require('path');

const servicesDir = 'C:\\Users\\MONSTER\\OneDrive\\Belgeler\\GitHub\\rentacar\\src\\app\\services';

const lookupServices = [
    { file: 'brand.service.ts', entity: 'brands', idProp: 'brand.brandId' },
    { file: 'color.service.ts', entity: 'colors', idProp: 'color.colorId' },
    { file: 'fuel.service.ts', entity: 'fuels', idProp: 'fuel.fuelId' },
    { file: 'gear.service.ts', entity: 'gears', idProp: 'gear.gearId' },
    { file: 'segment.service.ts', entity: 'segments', idProp: 'segment.id' },
    { file: 'location.service.ts', entity: 'locations', idProp: 'location.id' },
    { file: 'location-city.service.ts', entity: 'locationcities', idProp: 'locationCity.id' }
];

lookupServices.forEach(s => {
    const fullPath = path.join(servicesDir, s.file);
    if (fs.existsSync(fullPath)) {
        let content = fs.readFileSync(fullPath, 'utf8');

        // Replace getall
        content = content.replace(new RegExp('this\.apiUrl\\s*\\+\\s*["\']' + s.entity + '/getall["\']', 'gi'), 'this.apiUrl + "' + s.entity + '"');
        content = content.replace(new RegExp('this\.apiUrl\\s*\\+\\s*["\']getall["\']', 'gi'), 'this.apiUrl + "' + s.entity + '"'); // if they were just "getall"

        // Replace add
        content = content.replace(new RegExp('this\.apiUrl\\s*\\+\\s*["\']' + s.entity + '/add["\']', 'gi'), 'this.apiUrl + "' + s.entity + '"');
        content = content.replace(new RegExp('this\.apiUrl\\s*\\+\\s*["\']add["\']', 'gi'), 'this.apiUrl + "' + s.entity + '"');

        // Replace getbyid string concat
        content = content.replace(new RegExp('["\']https://localhost:44306/api/' + s.entity + '/getbyid\\?[a-zA-Z]+=["\']\\s*\\+\\s*([a-zA-Z0-9_]+)', 'gi'), 'this.apiUrl + "' + s.entity + '/" + ');
        content = content.replace(new RegExp('this\.apiUrl\\s*\\+\\s*["\']getbyid\\?[a-zA-Z]+=["\']\\s*\\+\\s*([a-zA-Z0-9_]+)', 'gi'), 'this.apiUrl + "' + s.entity + '/" + ');

        // Replace update
        // We know update was a POST to /update, now it is a PUT to /{id}.
        // We need to change .post<ResponseModel>(newUrl, entity) to .put<ResponseModel>(this.apiUrl + "entities/" + idProp, entity)
        // Find the "update(" function
        const updateRegex = new RegExp('update\\([^:]+:\\s*([^\\)]+)\\):\\s*Observable<ResponseModel>\\s*\\{\\s*(.*?)\\s*\\}', 'gis');
        content = content.replace(updateRegex, (match, type, body) => {
            // Find the object parameter name, e.g. "update(brand: Brand)"
            let argNameMatch = match.match(/update\(([a-zA-Z0-9_]+)\s*:/);
            let argName = argNameMatch ? argNameMatch[1] : 'entity';
            
            // Assume the properties from lookupServices match
            let idAccessor = s.idProp.replace(/^[^\.]+\./, argName + '.'); 

            return 'update(' + argName + ':' + type + '): Observable<ResponseModel> {\n    return this.httpClient.put<ResponseModel>(this.apiUrl + "' + s.entity + '/" + ' + idAccessor + ', ' + argName + ');\n  }';
        });

        fs.writeFileSync(fullPath, content, 'utf8');
        console.log("Updated: " + s.file);
    }
});
