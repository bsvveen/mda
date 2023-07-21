import React from 'react';

const useEntityModel = (entityName) => {

    const getEntityModel = () => {
        return new Promise((resolve, reject) => {    
            if (entityName == undefined)
                reject("entityName is undefined");

            const modelJson = sessionStorage.getItem("model");
            const model = JSON.parse(modelJson);
            if (model == undefined)
                reject("Model not found in sessionStorage", entityName);
        
            const entity = model.entities.find(e => e.name == entityName);   
            if (entity == undefined)
                reject("Entity not found in Model", entity);
            
            resolve(entity);
        });
    };    

    const [entityModel, setEntityModel] = React.useState(() => {
        getEntityModel().then((em) => setEntityModel(em)).catch((err) => {
            alert("Could not load EntityModel from SessionStorage, ", err.message);
            console.error(err);
        })
    });

    const setEntityModel2 = newModel => {
        try {
            window.sessionStorage.setItem("model", JSON.stringify(newModel));
        } catch (err) {
            alert(err);
        }
        setEntityModel(newValue);
    };

    return [entityModel, setEntityModel2];
};

export default useEntityModel;