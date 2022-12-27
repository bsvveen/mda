import React from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import DynamicForm from './dynamicform';

export default class Form extends React.Component {
    _isMounted = false;

    state = { entity: undefined, initial: undefined };

    PropTypes = {
        data: PropTypes.object,
        entity: PropTypes.string.isRequired,
        id: PropTypes.string,
        constrains: PropTypes.object,
        onCancel: PropTypes.func,
        onSubmit: PropTypes.func.isRequired,
        onDelete: PropTypes.func.isRequired         
    }    

    componentDidMount() {  
        this._isMounted = true; 

        this.repository = new Repository(this.props.entity);    
        this.setState({entity: this.getRequestedEntity()});   
        this.setState({initial: this.getInitialFromProps()});  
    }

    componentWillUnmount() {
        this._isMounted = false;
    }   

    onSubmit = (properties) => {    
        const dataToSubmit = Object.assign({}, { "entity": this.props.entity, "id": this.props.id }, { "properties": properties });
        return this.repository.Submit(dataToSubmit).then(this.props.onSubmit);  
    }
    
    onDelete = (id) => {    
        return this.repository.Delete(id).then(this.props.onDelete);  
    } 

    getRequestedEntity = () => {
        const model = JSON.parse(sessionStorage.getItem("model"));
        const entity = model.entities.find(e => e.name == this.props.entity);

        if (!entity)
            throw console.error("requested entity not found in model", this.props.entity);

        return entity;
    } 
    
    getInitialFromProps = () => {
        if (this.props.id)
            return this.props.data
       
        let initial = {};
        if (this.props.constrains) {
            initial = Object.keys(this.props.constrains).filter(constrain => constrain.Operator == 0).reduce((obj, key) => {
                obj[key] = this.props.constrains[key].value;
                return obj;
            }, {});
        }

        return initial;
    } 

    render() {      
        if (!this.state.entity || !this.state.initial)
            return <div>Loading</div>

        return <DynamicForm 
            initial = { this.state.initial }  
            constrains = { this.props.constrains }
            properties =  { this.state.entity.properties }
            onCancel =  { this.props.onCancel }
            onSubmit =  { this.onSubmit }
            onDelete =  { this.onDelete }           
            repository = { this.repository }                   
        />
    }
}