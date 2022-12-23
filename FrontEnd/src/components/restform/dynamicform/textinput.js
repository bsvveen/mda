import React from 'react';
import PropTypes from 'prop-types';

export default class TextInput extends React.Component {

    PropTypes = {
        model: PropTypes.object.isRequired,
        value: PropTypes.string.isRequired,       
        onChange: PropTypes.func.isRequired,
    }

    render() {
        var { model, value, onChange } = this.props;

        return (<input 
            className="input"
            type={model.type}
            key={model.key}
            name={model.name}
            value={value}
            onChange={(e) => { onChange(e, model.key) }}
        />);
    }
}