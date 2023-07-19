import React from 'react';
import DynamicForm from './dynamicform';
import { useFetchById, useUpdate } from './useDataApi';

const getEntityModel = (name) => {
  return new Promise((resolve, reject) => {
    const model = JSON.parse(sessionStorage.getItem("model"));
    if (model == undefined)
      reject("Model not found in sessionStorage", entity);

    const entity = model.entities.find(e => e.name == name);   
    if (entity == undefined)
      reject("Entity not found in Model", entity);
    
    resolve(entity);
})}

const formReducer = (state, action) => {
  switch (action.type) {
    case 'FETCH_EntityModel_INIT':
      return {
        ...state
      };
    case 'FETCH_EntityModel_SUCCES':
      return {
        ...state,
        entityModel: action.payload
      };     
    case 'FETCH_EntityInstance':
      return {
        ...state,        
        fetchResponse: action.payload
      };      
    case 'UPDATE_EntityInstance':
      return {
        ...state,
        updateResponse: action.payload
      };
    default:
      throw new Error();
  }
};


const Form = ({entityName, id, onClose}) => {  
  const [getResponse, setGetRequest] = useFetchById(entityName, id);
  const [updateResponse, setUpdateRequest] = useUpdate({}, undefined);  
  const [response, dispatch] = React.useReducer(formReducer, {   
    entityModel: null,   
    entityInstance: null,
    modelState: null
  });

  React.useEffect(() => {
    const fetchData = async () => {
      dispatch({ type: 'FETCH_EntityModel_INIT' });

      await getEntityModel(entityName).then((entityModel) => {
        dispatch({ type: 'FETCH_EntityModel_SUCCES', payload: entityModel });
      }).then(() => {

      }).catch((error) => alert(error))


     

      await apiFetch(request.url, request.payload)
      .then((res) => {
          if (res.status == "409")
            dispatch({ type: 'VALIDATION_FAILURE', payload: JSON.stringify(res.data) });

          dispatch({ type: 'FETCH_SUCCESS', payload: res.data });
      }) 
      .catch((error) => dispatch({ type: 'FETCH_FAILURE', payload: error.message + error.stack }))  
    }           

    fetchData();
  }, [request]);
 

  if (!entityModel) return <p>Loading...</p>;
  if (getResponse.loading || updateResponse.loading) return <p>Loading...</p>; 
  if (getResponse.error) return <p>{getResponse.error}</p>; 
  if (updateResponse.modelstate) return <p>{updateResponse.modelstate}</p>;  
  if (updateResponse.error) return <p>{updateResponse.error}</p>;    

  const onSubmit = (properties) => {   
    setUpdateRequest(entityName, id, properties);
    onClose();
  }  

  const onDelete = (id) => {    
    deleteMe(entityName, id);
    onClose();
  }    

  const onCancel = () => {    
    onClose();
  }  

  return (    
    <div className="form">
      <DynamicForm 
        id = { id }
        entityModel = { entityModel }  
        properties  = { getResponse.data }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;