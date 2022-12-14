import React from 'react';
import PropTypes from 'prop-types';

export default class TextInput extends React.Component {

    PropTypes = {
        contract: PropTypes.object.isRequired,
        value: PropTypes.string.isRequired,
        props: PropTypes.object,
        onChange: PropTypes.func.isRequired,
    }

    render() {
        var { contract, value, onChange } = this.props;

        return (<input {...contract.props}
            className="input"
            type={contract.type}
            key={contract.key}
            name={contract.name}
            value={value}
            onChange={(e) => { onChange(e, contract.key) }}
        />);
    }
}