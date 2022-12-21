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
import ItemProps from '../itemprops'

import './index.css';

export default class DynamicForm extends React.Component {

    state = { errors: [], modified: {} };

    PropTypes = {
        initial: PropTypes.object.isRequired,       
        model: PropTypes.object.isRequired,
        constrains: PropTypes.object,
        onCancel: PropTypes.func.isRequired,
        onSubmit: PropTypes.func.isRequired,
        onDelete: PropTypes.func.isRequired,
        repository: PropTypes.func.isRequired       
    }

    onDelete = (e) => {
        e.preventDefault();

        if (this.props.onDelete)
            this.props.onDelete(this.props.initial.id);       
    }

    onCancel = (e) => {
        e.preventDefault();

        if (this.props.onCancel)
            this.props.onCancel();       
    }

    onSubmit = (e) => {
        e.preventDefault();

        if (this.props.onSubmit) {
            let valuesToSubmit = Object.assign({}, this.props.initial, this.state.modified);
            this.props.onSubmit(valuesToSubmit)
            .then(() => this.setState({ "modified" : {} }))
            .catch((response) => {
                if (response.Errors) this.setState({ "errors": response.Errors });                
            });
        }
    }

    onChange = (e, key, type = "single") => {
        let value;
        if (type === "single") {
            value = e.target.value;
        } else {
            value = this.state.modified[key] || [];

            if (e.target.checked)
                value.push(e.target.value);

            if (!e.target.checked)
                value.splice(value.indexOf(e.target.value), 1);
        }

        if (value === "null")
            value = null;

        const modified = Object.assign({}, this.state.modified, { [key]: value });
        this.setState({ modified: modified });
    }

    renderForm = () => {
        let model = this.props.model;

        if (model) {
            let formUI = model.map((m) => {

                if (!m.Key || !m.Name)
                    throw console.error("model record is missing required property", m);

                let defaultValue = this.props.initial[m.Key] || "";
                let value = this.state.modified[m.Key] || defaultValue;
                let isReadonly = this.props.constrains && this.props.constrains.some(c => c.property == m.Key);
                let isHidden = isReadonly && m.Key.includes("_id");
                let type = m.Type || "Text";
                let errors = this.state.errors;
                let input = "";                   
                
                if (isHidden) // || m.props.disabled)
                    return null;

                if (isReadonly)
                    return (<div>{value}</div>);    

                if (type === "Text")
                    input = <TextInput contract={m} value={value} onChange={this.onChange} />

                if (type === "Number")
                    input = <NumberInput contract={m} value={value} onChange={this.onChange} />

                if (type === "textarea")
                    input = <TextArea contract={m} value={value} onChange={this.onChange} />

                if (type === "DateTime")
                    input = <DateInput contract={m} value={value} onChange={this.onChange} />               

                if (type === "select")
                    input = <SingleSelect contract={m} value={value} onChange={this.onChange} />

                if (type === "checkbox")
                    input = <CheckBox contract={m} value={value} onChange={this.onChange} />

                if (type === "foreignkey")
                    input = <Foreignkey contract={m} value={value} onChange={this.onChange} 
                    repository = {this.props.repository} 
                    constrains = {this.props.constrains} />

                if (type === "FKLookup")
                    input = <FKLookup contract={m} value={value} onChange={this.onChange} 
                    repository = {this.props.repository} 
                    constrains = {this.props.constrains} /> 

                return (
                    <div key={'g' + m.Key} className={type}>
                        <label key={"l" + m.Key} htmlFor={m.Key}>
                            {m.Name ? m.Name.replace("_id","") : m.Key}
                            {m.NotNull ? "*" : null}
                        </label>
                        {input}                        
                        <span className="error">{errors[m.Key] ? errors[m.Key] : ""}</span>                        
                    </div>
                );
            });

            return formUI;
        }
    }

    render() {
        return (
            <form className="dynamic-form" onSubmit={(e) => { this.onSubmit(e) }}>
                {this.renderForm()}
                <div className="actions">
                    {(this.props.initial.id) && <button type="delete" title="Verwijderen" onClick={this.onDelete} ></button>}
                    {(this.props.onCancel) && <button type="cancel" title="Cancel" onClick={this.onCancel}></button>}                    
                    <button type="submit" title="Opslaan"></button>
                </div>
                <ItemProps {...this.props.initial} />
            </form>
        )
    }
}