import React from 'react';
import PropTypes from 'prop-types';

import CheckBox from './checkbox';
import TextInput from './textinput';
import TextArea from './textarea';
import SingleSelect from './singleselect';
import DateInput from './date';
import Foreignkey from './foreignkey';
import FKLookup from './fklookup';
import NumberInput from './numberinput';

import './index.css';

export default class DynamicForm extends React.Component {
    state = { errors: [], modifiedData: {} };    

    onDelete = (e) => {
        e.preventDefault();

        if (this.props.onDelete)
            this.props.onDelete(this.props.id);       
    }

    onCancel = (e) => {
        e.preventDefault();

        if (this.props.onCancel)
            this.props.onCancel();       
    }

    onSubmit = (e) => {
        e.preventDefault();

        if (this.props.onSubmit) {
            let valuesToSubmit = Object.assign({}, this.props.properties, this.state.modifiedData);
            this.props.onSubmit(valuesToSubmit);
        }
    }

    onChange = (e, key, type = "single") => {
        let value;
        if (type === "single") {
            value = e.target.value;
        } else {
            value = this.state.modifiedData[key] || [];

            if (e.target.checked)
                value.push(e.target.value);

            if (!e.target.checked)
                value.splice(value.indexOf(e.target.value), 1);
        }

        if (value === "null")
            value = null;

        const modified = Object.assign({}, this.state.modifiedData, { [key]: value });
        this.setState({ modifiedData: modified });
    }

    renderForm = () => { 
        let formUI = this.props.entityModel.properties.map((prop) => {

            if (!prop.key || !prop.name)
                throw console.error("model record is missing required property", prop);

            let defaultValue = this.props.properties[prop.key] || "";
            let value = this.state.modifiedData[prop.key] || defaultValue;
            let isReadonly = this.props.constrains.some(c => c.property == prop.key);
            let isHidden = prop.key.includes("_id");
            let type = prop.type || "text";
            let errors = this.props.errors;
            let input = "";                   
            
            if (isHidden) // || m.props.disabled)
                return null;

            if (isReadonly)
                return (<div>{value}</div>);    

            if (type === "text")
                input = <TextInput model={prop} value={value} onChange={this.onChange} />

            if (type === "number")
                input = <NumberInput model={prop} value={value} onChange={this.onChange} />

            if (type === "textarea")
                input = <TextArea model={prop} value={value} onChange={this.onChange} />

            if (type === "datetime")
                input = <DateInput model={prop} value={value} onChange={this.onChange} />               

            if (type === "select")
                input = <SingleSelect model={prop} value={value} onChange={this.onChange} />

            if (type === "checkbox")
                input = <CheckBox model={prop} value={value} onChange={this.onChange} />

            if (type === "foreignkey")
                input = <Foreignkey model={prop} value={value} onChange={this.onChange} />

            if (type === "FKLookup")
                input = <FKLookup model={prop} value={value} onChange={this.onChange} 
                repository = {this.props.repository} 
                constrains = {this.props.constrains} /> 

            return (
                <div key={'g' + prop.key} className={type}>
                    <label key={"l" + prop.Key} htmlFor={prop.key}>
                        {prop.name ? prop.name.replace("_id","") : prop.key}
                        {prop.notnull ? "*" : null}
                    </label>
                    {input}                        
                    <span className="error">{errors[prop.key] ? errors[prop.key] : ""}</span>                        
                </div>
            );
        });

        return formUI;      
    }

    render() {
        return (
            <form className="dynamic-form" onSubmit={(e) => { this.onSubmit(e) }}>
                {this.renderForm()}
                <div className="actions">
                    {(this.props.id) && <button type="delete" title="Verwijderen" onClick={this.onDelete} >Delete</button>}
                    {(this.props.onCancel) && <button type="cancel" title="Cancel" onClick={this.onCancel}>Cancel</button>}                    
                    <button type="submit" title="Opslaan">Opslaan</button>
                </div>                
            </form>
        )
    }
}

DynamicForm.propTypes = {
    id: PropTypes.string,
    properties: PropTypes.object,      
    constrains: PropTypes.array,
    entityModel: PropTypes.object.isRequired,
    errors: PropTypes.array,
    onCancel: PropTypes.func.isRequired,
    onSubmit: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired     
};

DynamicForm.defaultProps = {
    properties: {},
    entityModel: {},
    constrains: []
};