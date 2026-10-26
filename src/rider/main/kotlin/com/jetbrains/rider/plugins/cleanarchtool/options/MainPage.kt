package com.jetbrains.rider.plugins.cleanarchtool.options

import com.jetbrains.rider.plugins.cleanarchtool.OptionPagesBundle
import com.jetbrains.rider.settings.simple.SimpleOptionsPage

class MainPage : SimpleOptionsPage(
    name = OptionPagesBundle.message("configurable.name.optionpages.options.title"),
    pageId = "MainPage" 
) {
    override fun getId(): String {
        return "MainPage"
    }
}