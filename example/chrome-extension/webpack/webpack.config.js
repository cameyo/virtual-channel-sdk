const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    mode: 'production',
    entry: {
        background: path.resolve(__dirname, "..", "src", "cameyo_vc_sdk_sample.ts"),
    },
    output: {
        path: path.join(__dirname, "../dist"),
        filename: "[name].js",
        clean: true,
    },
    resolve: {
        extensions: [".ts", ".js"],
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: "ts-loader",
                exclude: /node_modules/,
            },
        ],
    },
    plugins: [
        new CopyPlugin({
            patterns: [{ from: ".", to: ".", context: "public" }]
        }),
    ],
};
