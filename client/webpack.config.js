/* eslint-env node */
/* eslint-disable @typescript-eslint/no-var-requires */

const path = require('path');
const NpmDtsPlugin = require('npm-dts-webpack-plugin');

module.exports = {
  mode: 'production',
  entry: {
    cameyo_vc_sdk: path.resolve(__dirname, 'src', 'index.ts'),
  },
  output: {
    path: path.join(__dirname, './dist'),
    filename: 'sdk.js',
    clean: true,
    libraryTarget: 'umd',
    umdNamedDefine: true,
  },
  resolve: {
    extensions: ['.ts'],
  },
  devtool: 'source-map',
  optimization: {
    minimize: true,
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        loader: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  plugins: [
    new NpmDtsPlugin({
      output: path.resolve(__dirname, 'dist/index.d.ts'),
      logLevel: 'warn',
    }),
  ],
};
