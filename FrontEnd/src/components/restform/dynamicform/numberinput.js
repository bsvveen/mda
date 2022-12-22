import React from 'react';
import PropTypes from 'prop-types';

export default class NumberInput extends React.Component {

    PropTypes = {
        model: PropTypes.object.isRequired,
        value: PropTypes.string.isRequired,
        filter: PropTypes.string,        
        onChange: PropTypes.func.isRequired,
    }   

    render() {
        var { model, value, onChange } = this.props;

        return (<input 
            className="input"
            type={model.Type}            
            key={model.Key}
            name={model.Name}
            value={value}
            onChange={(e) => { onChange(e, model.Key) }}
        />);
    }
}