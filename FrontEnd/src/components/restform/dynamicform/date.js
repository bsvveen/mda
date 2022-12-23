import React from 'react';
import DatePicker  from 'react-datepicker';
import './date.css';

export default class DateInput extends React.Component { 
    
    formatValue(value) {  
        if (value)   
            return new Date(value);
    }

    onChange(value) {
        let e = { "target" : {"value" : value.toISOString() }}
        this.props.onChange(e, this.props.model.key)
    }

    render() {
        var { model, value } = this.props; 
        return <DatePicker className="input" key={model.key} selected={this.formatValue(value)} onChange={(value) => { this.onChange(value) }} />       
    }
}