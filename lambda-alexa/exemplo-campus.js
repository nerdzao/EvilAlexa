/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/
/**
 * This sample demonstrates a simple skill built with the Amazon Alexa Skills
 * nodejs skill development kit.
 * This sample supports multiple lauguages. (en-US, en-GB, de-DE).
 * The Intent Schema, Custom Slots and Sample Utterances for this skill, as well
 * as testing instructions are located at https://github.com/alexa/skill-sample-nodejs-fact
 **/

'use strict';

const Alexa = require('alexa-sdk');

const handlers = {
  LaunchRequest: function() {
    console.log('LaunchRequest');
    this.response.speak('On Campus Party, 1500 customers!');
    this.emit(':responseReady');
  },
  HowManyPeopleIsHere: function() {
    console.log('HowManyPeopleIsHere');
    this.response.speak('On Campus Party, 2502 customers!');
    this.emit(':responseReady');
  },
  Unhandled: function() {
    this.emit(':ask', 'HelpMessage', 'HelpMessage');
  },
};

exports.handler = function(event, context) {
  const alexa = Alexa.handler(event, context);

  alexa.registerHandlers(handlers);
  alexa.execute();
};
