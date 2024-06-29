using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class MainForm : Form
{
    private Dictionary dictionary;
    private RichTextBox textBox;
    private Button loadButton;
    private Button saveButton;
    private Button addWordButton;
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;

    public MainForm()
    {
        dictionary = new Dictionary();

        textBox = new RichTextBox { Dock = DockStyle.Fill };
        loadButton = new Button { Text = "Load Dictionary", Dock = DockStyle.Top };
        saveButton = new Button { Text = "Save Dictionary", Dock = DockStyle.Top };
        addWordButton = new Button { Text = "Add Word to Dictionary", Dock = DockStyle.Top };

        loadButton.Click += LoadButton_Click;
        saveButton.Click += SaveButton_Click;
        addWordButton.Click += AddWordButton_Click;
        textBox.TextChanged += TextBox_TextChanged;

        Controls.Add(textBox);
        Controls.Add(addWordButton);
        Controls.Add(loadButton);
        Controls.Add(saveButton);

        openFileDialog = new OpenFileDialog();
        saveFileDialog = new SaveFileDialog();
    }

    private void LoadButton_Click(object sender, EventArgs e)
    {
        openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            dictionary.LoadFromFile(openFileDialog.FileName);
        }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            dictionary.SaveToFile(saveFileDialog.FileName);
        }
    }

    private void AddWordButton_Click(object sender, EventArgs e)
    {
        string selectedWord = textBox.SelectedText.Trim();
        if (!string.IsNullOrEmpty(selectedWord))
        {
            dictionary.AddWord(selectedWord);
            HighlightWords();
        }
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        HighlightWords();
    }

    private void HighlightWords()
    {
        int selectionStart = textBox.SelectionStart;
        int selectionLength = textBox.SelectionLength;

        textBox.SelectAll();
        textBox.SelectionBackColor = Color.White;
        textBox.DeselectAll();

        string[] words = textBox.Text.Split(new[] { ' ', '.', ',', '!', '?', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            if (!dictionary.ContainsWord(word))
            {
                HighlightWord(word);
            }
        }

        textBox.SelectionStart = selectionStart;
        textBox.SelectionLength = selectionLength;
    }

    private void HighlightWord(string word)
    {
        int startIndex = 0;
        while ((startIndex = textBox.Text.IndexOf(word, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            textBox.Select(startIndex, word.Length);
            textBox.SelectionBackColor = Color.Yellow;
            startIndex += word.Length;
        }
        textBox.DeselectAll();
    }
}
